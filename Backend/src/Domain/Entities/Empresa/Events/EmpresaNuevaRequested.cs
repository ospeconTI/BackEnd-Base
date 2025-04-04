using MediatR;
using OSPeConTI.SumariosIERIC.Domain.Entities;
namespace OSPeConTI.SumariosIERIC.Domain.Events
{
    public class EmpresaNuevaRequested : INotification
    {
        public Empresa Empresa { get; private set; }
        public EmpresaNuevaRequested(Empresa empresa)
        {
            Empresa = empresa;
        }
    }
}
