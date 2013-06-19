if (typeof (AtlantisCaptcha) == "undefined") { // start single inclusion guard
    AtlantisCaptcha = function () {
        var _clickEvent = 'click.captcha';
        var _keypressEvent = 'keypress.captcha';
        var _reloadCaptchaEvent = 'captchaReload';
        var _playCaptchaSoundEvent = 'captchaPlaySound';
        var _ajaxValidateEvent = 'captchaAjaxValidate';
        var _imageLoadingEvent = 'captchaLoading';
        var _imageLoadedEvent = 'captchaLoaded';
        var _currentInstance;
        var _captchaId = '';
        var _instanceId = '';
        var _imageId = '';
        var _validateSelector = '';
        var _soundPlayingInProgress = false;
        var _reloadInProgress = false;
        var _autofocusInput = false;
        var _autoClearInput = false;
        var _millisecondsInAMinute = 60000;
        var _ajaxAuth = false;
        var _ajaxAuthenticateCallback;

        var Image;
        var ReloadLink;
        var PlaySoundLink;
        var CaptchaInput;

        this.Init = function (captchaId, instanceId, reloadLinkId,playSoundLinkId, inputId,validateSelector, autoFocusInput, autoClearInput, ajaxAuthenticate,ajaxAuthenticateCallback) {
            _currentInstance = this;
            _captchaId = captchaId;
            _instanceId = instanceId;
            _validateSelector = validateSelector;
            _ajaxAuth = ajaxAuthenticate.toLowerCase() == 'true';
            // Captcha image properties
            _imageId = captchaId + "_CaptchaImage";           
            Image = $('#' + _imageId);
            ReloadLink = $('#' + reloadLinkId);
            PlaySoundLink = $('#' + playSoundLinkId);
            CaptchaInput = $('#' + inputId);
            CaptchaInput.bind(_reloadCaptchaEvent, function () {
                _currentInstance.ReloadImage();
            });
            CaptchaInput.bind(_playCaptchaSoundEvent, function () {
                _currentInstance.PlaySound();
            });
            CaptchaInput.bind(_ajaxValidateEvent, function () {
                _currentInstance.Validate();
            });
            _autofocusInput = autoFocusInput;
            _autoClearInput = autoClearInput;
            PlaySoundLink.bind(_clickEvent, function () {
                _currentInstance.PlaySound();
            });
            ReloadLink.bind(_clickEvent, function () {
                _currentInstance.ReloadImage();                
            });
            var _validationURL = Image.attr('src').replace('get=image', 'get=validationResult');

            if (_ajaxAuth) {
                _ajaxAuthenticateCallback = ajaxAuthenticateCallback;
                $(_validateSelector).bind(_clickEvent, function () {                   
                    _currentInstance.Validate(_validationURL);
                });
            }
            if (_ajaxAuth) {
                CaptchaInput.bind(_keypressEvent, function (e) {
                    if (!e) var e = window.event;
                    if (e.keyCode) code = e.keyCode;
                    else if (e.which) code = e.which;
                    if (code == 13) {
                        _currentInstance.Validate(_validationURL);
                        return false;
                    }
                    return true;
                });
            }
        }

        this.ReloadImage = function () {
            if (Image && !_reloadInProgress) {
                var imageContainer = $('#' + _captchaId + "_CaptchaImageDiv");
                _reloadInProgress = true;
                DisableReloadIcon();
                var _loadingSpan = document.createElement('span');
                _loadingSpan.appendChild(document.createTextNode('loading...'));
                Image.remove();
                imageContainer.append(_loadingSpan);
                CaptchaInput.trigger(_imageLoadingEvent);
                var imageUrl = UpdateTimestamp(Image.attr('src'));
                var newImage = InitNewImage(imageUrl);
                newImage.bind('load.captcha', function () {
                    PostReloadImage(_loadingSpan);
                    EnableReloadIcon();
                });
                imageContainer.append(newImage);
                Image = newImage;
                if (_autofocusInput) {
                    FocusInput();
                }
                if (_autoClearInput) {
                    ClearInput();
                }
            }
        };

        var InitNewImage = function (imageUrl) {
            var _newImage = $(document.createElement('img'));
            _newImage.attr('id', Image.attr('id'));
            _newImage.attr('alt', Image.attr('alt'));
            _newImage.attr('src', imageUrl);
            return _newImage;
        }

        var PostReloadImage = function (loadingSpan) {
            $(loadingSpan).remove();
            _reloadInProgress = false;
            CaptchaInput.trigger(_imageLoadedEvent);
        };

        // CAPTCHA sound playing
        this.PlaySound = function () {
            StartSoundPlayback();
        }

        var StartSoundPlayback = function () {
            if (_soundPlayingInProgress) { return; }
            // ignore consecutive PlaySound() calls coming within very short intervals
            _soundPlayingInProgress = true;
            DisableSoundIcon();            

            var soundUrl = $('#' + _captchaId + "_SoundLink").attr('href');
            soundUrl = UpdateTimestamp(soundUrl);
            soundUrl = DetectSsl(soundUrl);
            var soundPlaceholderId = _captchaId + "_AudioPlaceholder";
            SoundPlaceholder = document.getElementById(soundPlaceholderId);
            SoundPlaceholder.innerHTML = '';

            document.body.style.cursor = 'wait';
            var html5SoundPlayed = false;
            var browserCompatibilityCheck = document.createElement('audio');
            if (!!(browserCompatibilityCheck.canPlayType) &&
                !!(browserCompatibilityCheck.canPlayType("audio/wav")) &&
                !DetectIncompetentBrowsers()) {
                sound = new Audio(soundUrl);
                sound.id = 'LBD_CaptchaSoundAudio';
                sound.type = 'audio/mpeg';
                sound.autoplay = true;
                sound.controls = false;
                sound.autobuffer = false;
                sound.loop = false;

                SoundPlaceholder.appendChild(sound);
                html5SoundPlayed = true;
            }

            if (!html5SoundPlayed) {
                var objectSrc = "<object id='LBD_CaptchaSoundObject' classid='clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95' height='0' width='0' style='width:0; height:0;'><param name='AutoStart' value='1' /><param name='Volume' value='0' /><param name='PlayCount' value='1' /><param name='FileName' value='" + soundUrl + "' /><embed id='LBD_CaptchaSoundEmbed' src='" + soundUrl + "' autoplay='true' hidden='true' volume='100' type='" + GetMimeType() + "' style='display:inline;' /></object>";

                SoundPlaceholder.innerHTML = objectSrc;
            }
            _soundPlayingInProgress = false;
            EnableSoundIcon();
            document.body.style.cursor = 'default';

        }

        // CAPTCHA Ajax validation
        this.Validate = function (validationURL) {
            var validationInput = CaptchaInput.val();
            var url = validationURL + '&i=' + validationInput;
            $.ajax({
                type:"get",
                dataType: "text",
                url: url,
                done:EndValidation,
                success: EndValidation
            });
        }

        var EndValidation = function (result) {
            var validationResult = result.toLowerCase() == 'true';
            if (!validationResult) {
                _currentInstance.ReloadImage();
            }
            if (_ajaxAuthenticateCallback != undefined) {
                _ajaxAuthenticateCallback(validationResult);
            }
        }           

        this.OnHelpLinkClick = function () {
        };

        // input processing
        var FocusInput = function () {
            var input = CaptchaInput[0];
            if (!_autofocusInput || !input) return;
            input.focus();
        }

        var ClearInput = function () {
            var input = CaptchaInput[0];
            if (!_autoClearInput || !input) return;
            input.value = '';
        }


        // helpers

        var UpdateTimestamp = function (url) {
            var i = url.indexOf('&d=');
            if (-1 !== i) {
                url = url.substring(0, i);
            }
            return url + '&d=' + GetTimestamp();
        }

        var GetTimestamp = function () {
            var d = new Date();
            var t = d.getTime() + (d.getTimezoneOffset() * _millisecondsInAMinute);
            return t;
        };

        var DetectSsl = function (url) {
            var i = url.indexOf('&e=');
            if (-1 !== i) {
                var len = url.length;
                url = url.substring(0, i) + url.substring(i + 4, len);
            }
            if (document.location.protocol === "https:") {
                url = url + '&e=1';
            }
            return url;
        }

        var GetMimeType = function () {
            var mimeType = "audio/x-wav";
            return mimeType;
        };

        var DetectIncompetentBrowsers = function () {
            return DetectFirefox3() || DetectSafariSsl() || DetectSafariMac();
        };

        var DetectFirefox3 = function () {
            var detected = false;
            if (navigator && navigator.userAgent) {
                var matches = navigator.userAgent.match(/(Firefox)\/(3\.6\.[^;\+,\/\s]+)/);
                if (matches) {
                    detected = true;
                }
            }
            return detected;
        };

        var DetectSafariSsl = function () {
            var detected = false;
            if (navigator && navigator.userAgent) {
                var matches = navigator.userAgent.match(/Safari/);
                if (matches) {
                    matches = navigator.userAgent.match(/Chrome/);
                    if (!matches && document.location.protocol === "https:") {
                        detected = true;
                    }
                }
            }
            return detected;
        };

        var DetectSafariMac = function () {
            var detected = false;
            if (navigator && navigator.userAgent) {
                var matches = navigator.userAgent.match(/Safari/);
                if (matches) {
                    matches = navigator.userAgent.match(/Chrome/);
                    if (!matches && navigator.userAgent.match(/Macintosh/)) {
                        detected = true;
                    }
                }
            }
            return detected;
        };

        var DisableReloadIcon = function () {
            ReloadLink.unbind(_clickEvent);
        }

        var EnableReloadIcon = function () {
            ReloadLink.bind(_clickEvent, function () {
                _currentInstance.ReloadImage();
            });
        }

        var DisableSoundIcon = function () {
            PlaySoundLink.unbind(_clickEvent);
        }

        var EnableSoundIcon = function () {
            PlaySoundLink.bind(_clickEvent, function () {
                _currentInstance.PlaySound();
            });
        }

    }
} // end single inclusion guard