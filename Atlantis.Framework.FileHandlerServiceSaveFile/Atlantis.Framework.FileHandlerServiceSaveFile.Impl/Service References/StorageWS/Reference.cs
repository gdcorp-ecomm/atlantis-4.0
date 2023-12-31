﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="StorageWS.IIntakeEcho")]
    public interface IIntakeEcho {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIntakeEcho/Echo", ReplyAction="http://tempuri.org/IIntakeEcho/EchoResponse")]
        string Echo(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIntakeEchoChannel : Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeEcho, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IntakeEchoClient : System.ServiceModel.ClientBase<Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeEcho>, Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeEcho {
        
        public IntakeEchoClient() {
        }
        
        public IntakeEchoClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IntakeEchoClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntakeEchoClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntakeEchoClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Echo(string message) {
            return base.Channel.Echo(message);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="StorageWS.IIntakeService")]
    public interface IIntakeService {
        
        // CODEGEN: Generating message contract since the wrapper name (IntakeMessage) of message IntakeMessage does not match the default value (SaveFile)
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIntakeService/SaveFile", ReplyAction="http://tempuri.org/IIntakeService/SaveFileResponse")]
        Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeResponse SaveFile(Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessage request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="IntakeMessage", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class IntakeMessage {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string ApplicationData;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string ApplicationKey;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string FileName;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public int SettingsSet;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public int SubscriberId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.IO.Stream FileData;
        
        public IntakeMessage() {
        }
        
        public IntakeMessage(string ApplicationData, string ApplicationKey, string FileName, int SettingsSet, int SubscriberId, System.IO.Stream FileData) {
            this.ApplicationData = ApplicationData;
            this.ApplicationKey = ApplicationKey;
            this.FileName = FileName;
            this.SettingsSet = SettingsSet;
            this.SubscriberId = SubscriberId;
            this.FileData = FileData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="IntakeResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class IntakeResponse {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public bool Success;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string Message;
        
        public IntakeResponse() {
        }
        
        public IntakeResponse(bool Success, string Message) {
            this.Success = Success;
            this.Message = Message;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIntakeServiceChannel : Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IntakeServiceClient : System.ServiceModel.ClientBase<Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeService>, Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeService {
        
        public IntakeServiceClient() {
        }
        
        public IntakeServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IntakeServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntakeServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntakeServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeResponse Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeService.SaveFile(Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessage request) {
            return base.Channel.SaveFile(request);
        }
        
        public bool SaveFile(string ApplicationData, string ApplicationKey, string FileName, int SettingsSet, int SubscriberId, System.IO.Stream FileData, out string Message) {
            Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessage inValue = new Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessage();
            inValue.ApplicationData = ApplicationData;
            inValue.ApplicationKey = ApplicationKey;
            inValue.FileName = FileName;
            inValue.SettingsSet = SettingsSet;
            inValue.SubscriberId = SubscriberId;
            inValue.FileData = FileData;
            Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeResponse retVal = ((Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeService)(this)).SaveFile(inValue);
            Message = retVal.Message;
            return retVal.Success;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="StorageWS.IIntakeServiceRemoteFile")]
    public interface IIntakeServiceRemoteFile {
        
        // CODEGEN: Generating message contract since the wrapper name (IntakeMessageFilePath) of message IntakeMessageFilePath does not match the default value (AcceptFile)
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IIntakeServiceRemoteFile/AcceptFile", ReplyAction="http://tempuri.org/IIntakeServiceRemoteFile/AcceptFileResponse")]
        Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeResponse AcceptFile(Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessageFilePath request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="IntakeMessageFilePath", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class IntakeMessageFilePath {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string ApplicationData;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string ApplicationKey;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string FileName;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public int SettingsSet;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public int SubscriberId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string FilePath;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public int FileSize;
        
        public IntakeMessageFilePath() {
        }
        
        public IntakeMessageFilePath(string ApplicationData, string ApplicationKey, string FileName, int SettingsSet, int SubscriberId, string FilePath, int FileSize) {
            this.ApplicationData = ApplicationData;
            this.ApplicationKey = ApplicationKey;
            this.FileName = FileName;
            this.SettingsSet = SettingsSet;
            this.SubscriberId = SubscriberId;
            this.FilePath = FilePath;
            this.FileSize = FileSize;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IIntakeServiceRemoteFileChannel : Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeServiceRemoteFile, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IntakeServiceRemoteFileClient : System.ServiceModel.ClientBase<Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeServiceRemoteFile>, Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeServiceRemoteFile {
        
        public IntakeServiceRemoteFileClient() {
        }
        
        public IntakeServiceRemoteFileClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IntakeServiceRemoteFileClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntakeServiceRemoteFileClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IntakeServiceRemoteFileClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeResponse Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeServiceRemoteFile.AcceptFile(Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessageFilePath request) {
            return base.Channel.AcceptFile(request);
        }
        
        public bool AcceptFile(string ApplicationData, string ApplicationKey, string FileName, int SettingsSet, int SubscriberId, string FilePath, int FileSize, out string Message) {
            Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessageFilePath inValue = new Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeMessageFilePath();
            inValue.ApplicationData = ApplicationData;
            inValue.ApplicationKey = ApplicationKey;
            inValue.FileName = FileName;
            inValue.SettingsSet = SettingsSet;
            inValue.SubscriberId = SubscriberId;
            inValue.FilePath = FilePath;
            inValue.FileSize = FileSize;
            Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IntakeResponse retVal = ((Atlantis.Framework.FileHandlerServiceSaveFile.Impl.StorageWS.IIntakeServiceRemoteFile)(this)).AcceptFile(inValue);
            Message = retVal.Message;
            return retVal.Success;
        }
    }
}
