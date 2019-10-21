#
# build.ps1
#
dotnet ef dbcontext scaffold "Data Source=192.168.30.31; Initial Catalog=AbsCoreDevelopment; User Id=sa;pwd=fkt" Microsoft.EntityFrameworkCore.SqlServer -o Models\Core -c AbsContext -d --force