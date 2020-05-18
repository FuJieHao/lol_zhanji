<%@ Page Language="C#" Inherits="lol.Default" %>
<!DOCTYPE html>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<html>
<head runat="server">
</head>
<body>
    <form id="head" runat="server"> 
    <div>
        <asp:Button id="create" runat="server" Text="添加一行数据" OnClick="create_data"/>
        <asp:Button id="delete" runat="server" Text="删除一行数据" OnClick="delete_data"/>
    </div>
    </form>
        
    <form id="tableView" runat="server">
        <asp:Table id="table_data" runat="server"></asp:Table>
    </form>
        

</body>
</html>
