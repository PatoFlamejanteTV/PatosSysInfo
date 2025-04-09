Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.IO

Public Class Cool
    ' Obtém informações gerais do sistema operacional
    Public Shared Function GetOS() As String
        Dim osName As String = RuntimeInformation.OSDescription
        Dim osVersion As String = Environment.OSVersion.ToString()
        Dim architecture As String = RuntimeInformation.OSArchitecture.ToString()
        Dim osDetails As String = $"OS: {osName} {osVersion}, Architecture: {architecture}"

        ' Se for Linux, obter distribuição
        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            osDetails &= $", Distro: {GetLinuxDistro()}"
        End If

        ' Adicionar detalhes do terminal (se em Linux)
        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            osDetails &= $", Terminal: {GetTerminalInfo()}"
        End If

        Return osDetails
    End Function

    ' Método para pegar informações de distribuição de Linux
    Private Shared Function GetLinuxDistro() As String
        Try
            If File.Exists("/etc/os-release") Then
                Dim lines = File.ReadAllLines("/etc/os-release")
                For Each line In lines
                    If line.StartsWith("PRETTY_NAME=") Then
                        Dim distro = line.Replace("PRETTY_NAME=", "").Replace("""", "").Trim()
                        Return distro
                    End If
                Next
            End If
        Catch ex As Exception
            ' Ignorar qualquer erro relacionado à leitura do arquivo
        End Try
        Return "Desconhecida"
    End Function

    ' Método para obter informações do terminal em Linux
    Private Shared Function GetTerminalInfo() As String
        Try
            Dim terminal = Environment.GetEnvironmentVariable("TERM")
            If Not String.IsNullOrWhiteSpace(terminal) Then
                Return terminal
            End If
        Catch ex As Exception
            ' Ignorar erros na obtenção da variável de ambiente
        End Try
        Return "Desconhecido"
    End Function
End Class