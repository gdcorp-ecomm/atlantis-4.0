<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:a="http://schemas.microsoft.com/2003/10/Serialization/Arrays" exclude-result-prefixes="msxsl">

  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html>
      <head>
        <title>Unit Test Results</title>
        <style type="text/css" media="screen">
          .bold { font-weight: bold; }
          .pass { color: Green;  }
          .fail { color: Red; }
          .ignore { color: Gold; }
          .rounded {  height: 16px; width: 16px; -moz-border-radius: 8px; border-radius: 8px; }
          .rounded.pass { background-color: Green; }
          .rounded.fail { background-color: Red; }
          .rounded.ignore { background-color: Gold; }
          .row{ min-width:100%;}
          .col {display: table-cell; width:100px; margin:0px; padding:0px;}
          .col-sm {display: table-cell; width:40px;}
          .col-lg {display: table-cell; width:400px;}
          .border {border:solid thin black;}
          .border-lt {border-left:solid thin black;}
          .border-rt {border-right:solid thin black;}
          .border-bt {border-bottom:solid thin black;}
          .border-tp {border-top:solid thin black;}
          .testDetailItem {display:none; margin-top:15px; color:black; font-weight:normal; font-size:.9em;}
          .toggleText {font-size:.5em;color:blue;cursor:pointer;}
          .test { border-bottom: solid thin black; }
          .no-margin {margin:0;}
          .pad-lt { padding-left:60px; }
          .pad-lt-sm { padding-left:10px; }
          .pad-tp-sm { padding-top:5px; }
          #summary{ width:100%; margin-top:25px;}
          #summary .col { width: 250px; }
        </style>
      </head>
      <body>
        <xsl:apply-templates select="//TestResults" />
        <xsl:apply-templates select="//TestResults" mode="summary"  />


        <script type="text/javascript">

          function toggleTestDetails(item) {
          if (item.getElementsByClassName('toggleText')[0].innerText == "Show Details") {
          item.getElementsByClassName('toggleText')[0].innerText="Hide Details";
          item.getElementsByClassName('testDetailItem')[0].style.display = "block";
          } else {
          item.getElementsByClassName('toggleText')[0].innerText="Show Details";
          item.getElementsByClassName('testDetailItem')[0].style.display = "none";
          }
          }

        </script>

      </body>
    </html>


  </xsl:template>

  <xsl:template match="//TestResults">

    <div id="testResults" class="border">
      <div class="row border-bt no-margin">
        <div class="col-lg bold pad-lt">Test Name</div>
        <div class="col bold pad-lt-sm">Status</div>
        <div class="col-lg bold">Results</div>
      </div>

      <xsl:for-each select="./TestResult">
        <xsl:variable name="status">
          <xsl:choose>
            <xsl:when test="./Success/text() = 'false'">Fail</xsl:when>
            <xsl:when test="./Success/text() = 'true'">Pass</xsl:when>
            <xsl:otherwise>Ignore</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <div class="test">
          <div>
            <xsl:attribute name="class">
              <xsl:choose>
                <xsl:when test="$status = 'Ignore'">
                  <xsl:text>row ignore bold</xsl:text>
                </xsl:when>
                <xsl:when test="$status = 'Pass'">
                  <xsl:text>row pass bold</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:text>row fail bold</xsl:text>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>

            <xsl:attribute name="style">
              <xsl:choose>
                <xsl:when test="(position() mod 2) != 1">
                  <xsl:text>background-color: #CCC;</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:text>background-color: #FFF;</xsl:text>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>

            <div class="col-sm pad-tp-sm pad-lt-sm">
              <div>
                <xsl:attribute name="class">
                  <xsl:choose>
                    <xsl:when test="$status = 'Ignore'">
                      <xsl:text>ignore rounded</xsl:text>
                    </xsl:when>
                    <xsl:when test="$status = 'Pass'">
                      <xsl:text>pass rounded</xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:text>fail rounded</xsl:text>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:text disable-output-escaping="yes"> </xsl:text>
              </div>
            </div>

            <div class="col-lg pad-lt-sm border-lt">
              <xsl:value-of select="./TestName/text()"/>
            </div>
            <div class="col pad-lt-sm border-lt">
              <xsl:value-of select="$status"/>
            </div>
            <div class="col-lg pad-lt-sm border-lt">
              <xsl:value-of select="./Result/text()"/>
              <xsl:call-template  name="extLogData">
                <xsl:with-param name="test-name" select="./TestName/text()"/>
              </xsl:call-template>
            </div>
          </div>
        </div>
      </xsl:for-each>
    </div>
  </xsl:template>

  <xsl:template name="extLogData" match="//ExtendedLogData">
    <xsl:param name="test-name" />
    <xsl:variable name="log" select="//ExtendedLogData/LogItem[Key=$test-name]"/>

    <xsl:if test="count($log) > 0">
      <div class="testDetails" onclick="toggleTestDetails(this);">
        <span class="toggleText">Show Details</span>
        <div class="testDetailItem" style="overflow:auto;width:550px;height:200px;">
          <xsl:for-each select="$log/Value/Entry">
            <xsl:value-of select="./text()"/>
            <br />
          </xsl:for-each>
        </div>
      </div>
    </xsl:if>
  </xsl:template>

  <xsl:template match="//TestResults" mode="summary">
    <xsl:variable name="totalRun" select="count(//TestResult)"></xsl:variable>
    <xsl:variable name="totalPassed" select="count(//TestResult[Success ='true'])"></xsl:variable>
    <xsl:variable name="totalFailed" select="count(//TestResult[Success ='false'])"></xsl:variable>
    <xsl:variable name="totalIgnored" select="count(//TestResult[Success =''])"></xsl:variable>

    <div id="summary" data-totalrun="{$totalRun}" data-totalpassed="{$totalPassed}" data-totalfailed="{$totalFailed}" data-totalignored="{$totalIgnored}" class="border">
      <div class="row">
        <div class="col-lg bold pad-lt-sm">Test Summary</div>
      </div>
      <div class="row border-tp">
        <div class="col bold pad-lt-sm">
          <xsl:value-of select="$totalRun"  /> Tests Run
        </div>
        <div class="col pass bold border-lt pad-lt-sm">
          <xsl:value-of select="$totalPassed"  /> Tests Passed
        </div>
        <div class="col fail bold border-lt pad-lt-sm">
          <xsl:value-of select="$totalFailed"  /> Tests Failed
        </div>
        <div class="col ignore bold border-lt pad-lt-sm">
          <xsl:value-of select="$totalIgnored"  /> Tests Ignored
        </div>
      </div>
    </div>
  </xsl:template>

</xsl:stylesheet>
