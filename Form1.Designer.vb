<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        btnDLLLoad = New Button()
        btnDeviceConn = New Button()
        btnDeviceSearchConn = New Button()
        SuspendLayout()
        ' 
        ' btnDLLLoad
        ' 
        btnDLLLoad.Location = New Point(25, 24)
        btnDLLLoad.Name = "btnDLLLoad"
        btnDLLLoad.Size = New Size(130, 44)
        btnDLLLoad.TabIndex = 0
        btnDLLLoad.Text = "SDK 초기화"
        btnDLLLoad.UseVisualStyleBackColor = True
        ' 
        ' btnDeviceConn
        ' 
        btnDeviceConn.Location = New Point(175, 24)
        btnDeviceConn.Name = "btnDeviceConn"
        btnDeviceConn.Size = New Size(130, 44)
        btnDeviceConn.TabIndex = 0
        btnDeviceConn.Text = "IP주소로 장치연결"
        btnDeviceConn.UseVisualStyleBackColor = True
        ' 
        ' btnDeviceSearchConn
        ' 
        btnDeviceSearchConn.Location = New Point(327, 24)
        btnDeviceSearchConn.Name = "btnDeviceSearchConn"
        btnDeviceSearchConn.Size = New Size(130, 44)
        btnDeviceSearchConn.TabIndex = 0
        btnDeviceSearchConn.Text = "장치 검색후 연결"
        btnDeviceSearchConn.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(btnDeviceSearchConn)
        Controls.Add(btnDeviceConn)
        Controls.Add(btnDLLLoad)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnDLLLoad As Button
    Friend WithEvents btnDeviceConn As Button
    Friend WithEvents btnDeviceSearchConn As Button

End Class
