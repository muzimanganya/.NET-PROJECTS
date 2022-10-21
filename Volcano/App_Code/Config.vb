Imports System.Configuration
Public Class Config
    Public ReadOnly Property SystemID As String
        Get
            Return ConfigurationManager.AppSettings("SMSCSystemID")
        End Get
    End Property
    Public ReadOnly Property Password As String
        Get
            Return ConfigurationManager.AppSettings("SMSCPassword")
        End Get
    End Property
    Public ReadOnly Property port As Int32
        Get
            Return ConfigurationManager.AppSettings("SMSCPort")
        End Get
    End Property
    Public ReadOnly Property Host As String
        Get
            Return ConfigurationManager.AppSettings("SMSCHost")
        End Get
    End Property
    Public ReadOnly Property SystemType As String
        Get
            Return ConfigurationManager.AppSettings("SMSCSystemType")
        End Get
    End Property
    Public ReadOnly Property DefaultServiceType As String
        Get
            Return ConfigurationManager.AppSettings("SMSCDefaultServiceType")
        End Get
    End Property
    Public ReadOnly Property AutoReconnectDelay As Int32
        Get
            Return ConfigurationManager.AppSettings("SMSCAutoReconnectDelay")
        End Get
    End Property
    Public ReadOnly Property KeepAliveInterval As Int32
        Get
            Return ConfigurationManager.AppSettings("SMSCKeepAlive")
        End Get
    End Property
    Public ReadOnly Property SourcePhoneNumber As Int32
        Get
            Return ConfigurationManager.AppSettings("SMSCSourcePhoneNumber")
        End Get
    End Property
    Public ReadOnly Property SQLServer As String
        Get
            Return myEncryption.Decrypt(ConfigurationManager.AppSettings("SQLServer"), "Ey5TXu2WLMKvpa")
        End Get
    End Property
    Public ReadOnly Property SQLUser As String
        Get
            Return myEncryption.Decrypt(ConfigurationManager.AppSettings("SQLUser"), "Ey5TXu2WLMKvpa")
        End Get
    End Property
    Public ReadOnly Property SQLPassword As String
        Get
            Return myEncryption.Decrypt(ConfigurationManager.AppSettings("SQLPass"), "Ey5TXu2WLMKvpa")
        End Get
    End Property
    Public ReadOnly Property SecurityDB As String
        Get
            Return myEncryption.Decrypt(ConfigurationManager.AppSettings("SecurityDB"), "Ey5TXu2WLMKvpa")
        End Get
    End Property
    Public ReadOnly Property CommissionPercentage As String
        Get
            Return ConfigurationManager.AppSettings("CommissionPercentage")
        End Get
    End Property
    Public ReadOnly Property WebServiceUserName As String
        Get
            Return ConfigurationManager.AppSettings("WebServiceUserName")
        End Get
    End Property
    Public ReadOnly Property WebServicePassword As String
        Get
            Return ConfigurationManager.AppSettings("WebServicePassword")
        End Get
    End Property
End Class
