using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Netlab.Domain.DTOs
{
    public class ResultadosArbovirosisRequest {
        public string dni { get; set; }
    }
    public class ResultadosArbovirosis
    {
        public string Fechasolicitud { get; set; }
        public string Codigoorden { get; set; }
        public string Codificacion { get; set; }
        public string Codigolineal { get; set; }
        public string Oficio { get; set; }
        public string Eessdeporigen { get; set; }
        public string Eessprovorigen { get; set; }
        public string Eessdistorigen { get; set; }
        public string Eesssubsector { get; set; }
        public string Eessdisaorigen { get; set; }
        public string Eessredorigen { get; set; }
        public string Eessmicroredorigen { get; set; }
        public string Codigorenipressorigen { get; set; }
        public string Establecimientoorigen { get; set; }
        public string Eessdepdestino { get; set; }
        public string Eessprovdestino { get; set; }
        public string Eessdistdestino { get; set; }
        public string Eessdisadestino { get; set; }
        public string Eessreddestino { get; set; }
        public string Eessmicroreddestino { get; set; }
        public string Codigorenipressdestino { get; set; }
        public string Eesslabdestino { get; set; }
        public string Docidentidad { get; set; }
        public string Departamentopacientereniec { get; set; }
        public string Provinciapacientereniec { get; set; }
        public string Distritopacientereniec { get; set; }
        public string Departamentopacienteactual { get; set; }
        public string Provinciapacienteactual { get; set; }
        public string Distritopacienteactual { get; set; }
        public string Direccionreniec { get; set; }
        public string Direccionactual { get; set; }
        public string Fechanacimiento { get; set; }
        public string Nombrepaciente { get; set; }
        public string Ocupacion { get; set; }
        public string Edad { get; set; }
        public string Sexopaciente { get; set; }
        public string Tipomuestra { get; set; }
        public string Enfermedad { get; set; }
        public string Nombreexamen { get; set; }
        public string Componente { get; set; }
        public string Fecharegistro { get; set; }
        public string Fechahoracoleccion { get; set; }
        public string Fecharecepcionrom { get; set; }
        public string Fechahorarecepcionlab { get; set; }
        public string Fechahoravalidado { get; set; }
        public string Muestraconforme { get; set; }
        public string Criteriosrechazo { get; set; }
        public string Convresultado { get; set; }
        public string Estatusresultado { get; set; }
        public string Dxmoleculardengue { get; set; }
        public string Serotipo { get; set; }
        public string Elisans1 { get; set; }
        public string Elisaigg { get; set; }
        public string Elisaigm { get; set; }
        public string Priggeigm { get; set; }
        public string Prns1 { get; set; }
        public string Artralgiasmanos { get; set; }
        public string Artralgiaspies { get; set; }
        public string Cefalea { get; set; }
        public string Chikungunyagrave { get; set; }
        public string Chikungunya { get; set; }
        public string Cianosis { get; set; }
        public string Clasificaciondecaso { get; set; }
        public string Compromisodeorganos { get; set; }
        public string Conjuntivitisnopurulenta { get; set; }
        public string Dengueconsignosdealarma { get; set; }
        public string Denguegrave { get; set; }
        public string Denguesinsignosdealarma { get; set; }
        public string Departamento { get; set; }
        public string Derrameseroso { get; set; }
        public string Dirección { get; set; }
        public string Disminuciondediuresis { get; set; }
        public string Disminuciondeplaquetas { get; set; }
        public string Disnea { get; set; }
        public string Distrito { get; set; }
        public string Dolorabdominal { get; set; }
        public string Dolorarticular { get; set; }
        public string Dolorlumbar { get; set; }
        public string Dolorocularoretroocular { get; set; }
        public string Dolortoraxico { get; set; }
        public string Enfermedadcoexistente { get; set; }
        public string Episodioanteriordedengue { get; set; }
        public string Escaladeglasgow { get; set; }
        public string Estadomentalalterado { get; set; }
        public string Fallecido { get; set; }
        public string Fechadeiniciodesintomas { get; set; }
        public string Fechadepermanenciadesde { get; set; }
        public string Fechadepermanenciahasta { get; set; }
        public string Fechadetomaprimeramuestra { get; set; }
        public string Fechadetomasegundamuestra { get; set; }
        public string Fecha { get; set; }
        public string Fecha1 { get; set; }
        public string Gestante { get; set; }
        public string Hepatomegalia { get; set; }
        public string Hipotermia { get; set; }
        public string Hospitalizado { get; set; }
        public string Ictericia { get; set; }
        public string Incrementodehematocrito { get; set; }
        public string Localidad { get; set; }
        public string Mayaro { get; set; }
        public string Mialgias { get; set; }
        public string Nauseas { get; set; }
        public string Oropuche { get; set; }
        public string Pais { get; set; }
        public string Presionarterial { get; set; }
        public string Provincia { get; set; }
        public string Pulsodebil { get; set; }
        public string Referido { get; set; }
        public string Sangrado { get; set; }
        public string Semanas { get; set; }
        public string Subsistemadevigilancia { get; set; }
        public string Temperatura { get; set; }
        public string Tipodecaso { get; set; }
        public string Vacunaantiamarilica { get; set; }
        public string Vomitos { get; set; }
        public string Zika { get; set; }
    }
}
