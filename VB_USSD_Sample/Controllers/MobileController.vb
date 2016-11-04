Imports System.Net
Imports System.Net.Http
Imports System.Web.Http

Namespace Controllers

    ' you need to explicitly declare the parameters that the server will send to your app
    ' the server returns phoneNumber, text, sessionId and serviceCode 
    ' this class acts as complex type to facilitate parameter binding
    ' make getters and setters for the values you expect from the server
    Public Class ServerResponse
        Public Property text() As String
            Get
                Return m_text
            End Get
            Set
                m_text = Value
            End Set
        End Property
        Private m_text As String
        Public Property phoneNumber() As String
            Get
                Return m_phoneNumber
            End Get
            Set
                m_phoneNumber = Value
            End Set
        End Property
        Private m_phoneNumber As String
        Public Property sessionId() As String
            Get
                Return m_sessionId
            End Get
            Set
                m_sessionId = Value
            End Set
        End Property
        Private m_sessionId As String
        Public Property serviceCode() As String
            Get
                Return m_serviceCode
            End Get
            Set
                m_serviceCode = Value
            End Set
        End Property
        Private m_serviceCode As String
    End Class



    <RoutePrefix("api/mobile")>  ' specify the route prefix so that your route can look like...  localhost:8080/api/mobile...
    Public Class MobileController
        Inherits ApiController 'the controller should inherit from the ApiController class

        <Route("ussd")>         ' specify the actual route, your url will look like... localhost:8080/api/mobile/ussd...
        <HttpPost, ActionName("ussd")> ' state that the method you intend to create is a POST 
        Public Function ussd(<FromBody> ServerResponse As ServerResponse) As HttpResponseMessage ' declare a complex type as input parameter
            Dim rs As HttpResponseMessage
            Dim response As String

            ' the server may return text parameter as null and hence the need to initialize it to an empty string
            If ServerResponse.text Is Nothing Then
                ServerResponse.text = ""
            End If

            ' loop through the server's text value to determine the next cause of action
            If ServerResponse.text.Equals("", StringComparison.Ordinal) Then
                response = "CON This is AfricasTalking \n" + ' always include a 'CON' in your first statements
                    "1. Get you phone number"
            ElseIf ServerResponse.text.Equals("1", StringComparison.Ordinal) Then
                response = "END Your phone number is " + ServerResponse.phoneNumber 'the last response starts with an 'END' so that the server understands that its the final response
            Else
                response = "END bad option"
            End If

            ' all 'POST' operations should return status code of 201 (HttpStatusCode.Created == 201) and a representation of the value
            rs = Request.CreateResponse(HttpStatusCode.Created, response)

            ' append your response to the HttpResponseMessage and set content type to text/plain, exactly what the server expects
            rs.Content = New StringContent(response, Encoding.UTF8, "text/plain")
            ' finally return your response to the server
            Return rs

        End Function


    End Class


End Namespace