
using NPoco;
using System.ComponentModel.DataAnnotations;

namespace Netlab.Domain.Entities
{
    public class DatoNotiwebNetlabResponse
    {
        public int responseId { get; set; }
        public string codigo_cdc { get; set; }
        public string codigo_INS { get; set; }
    }

    [TableName("[Interoperabilidad].[DatoNotiwebNetlab]")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class DatoNotiwebNetlab
    {
        public int Id { get; set; }

        //ESTABLECIMIENTO NOTIFICANTE
        [Required(ErrorMessage = "El campo cod_estab_salud_est es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo cod_estab_salud_est no debe exceder los 50 caracteres.")]
        public string cod_estab_salud_est { get; set; }

        //PACIENTE
        [Required(ErrorMessage = "El campo cod_tipo_doc_pac es obligatorio")]
        [MaxLength(2, ErrorMessage = "El campo cod_tipo_doc_pac no debe exceder los 2 caracteres.")]
        public string cod_tipo_doc_pac { get; set; }
        
        [Required(ErrorMessage = "El campo nro_doc_pac es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo nro_doc_pac no debe exceder los 50 caracteres.")]
        public string nro_doc_pac { get; set; }

        [Required(ErrorMessage = "El campo apepat_pac es obligatorio")]
        [MaxLength(150, ErrorMessage = "El campo apepat_pac no debe exceder los 150 caracteres.")]
        public string apepat_pac { get; set; }
        
        [Required(ErrorMessage = "El campo apemat_pac es obligatorio")]
        [MaxLength(150, ErrorMessage = "El campo apemat_pac no debe exceder los 150 caracteres.")]
        public string apemat_pac { get; set; }
        
        [Required(ErrorMessage = "El campo nombres_pac es obligatorio")]
        [MaxLength(150, ErrorMessage = "El campo nombres_pac no debe exceder los 150 caracteres.")]
        public string nombres_pac { get; set; }
        
        [MaxLength(1, ErrorMessage = "El campo sexo_pac no debe ser mayor a 1 caracter.")]
        public string? sexo_pac { get; set; }
        
        [MaxLength(50, ErrorMessage = "El campo cod_nacionalidad_pac no debe exceder los 50 caracteres.")]
        public string? cod_nacionalidad_pac { get; set; }
        
        [MaxLength(50, ErrorMessage = "El campo cod_etnia_pac no debe exceder los 50 caracteres.")]
        public string? cod_etnia_pac { get; set; }
        
        [MaxLength(50, ErrorMessage = "El campo cod_etniaproc_pac no debe exceder los 50 caracteres.")]
        public string? cod_etniaproc_pac { get; set; }
        
        [MaxLength(50, ErrorMessage = "El campo otroetniaproc_pac no debe exceder los 50 caracteres.")]
        public string? otroetniaproc_pac { get; set; }//si la etnia elegida es OTRO, es obligatorio el registro de otra Etnia
        
        [MaxLength(50, ErrorMessage = "El campo hist_clinica_pac no debe exceder los 50 caracteres.")]
        public string? hist_clinica_pac { get; set; }
        public DateOnly? fecha_hos_pac { get; set; }
        
        [Required(ErrorMessage = "El campo tipo_edad_pac es obligatorio")]
        [MaxLength(1, ErrorMessage = "El campo tipo_edad_pac no debe exceder los 50 caracteres.")]
        [RegularExpression("(?i)^(A|M|D)$", ErrorMessage = "El campo tipo_edad_pac debe ser 'A' (Años), 'M' (Meses) o 'D' (Días).")]
        public string tipo_edad_pac { get; set; }
        
        [Required(ErrorMessage = "El campo edad_pac es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo edad_pac no debe exceder los 50 caracteres.")]
        public string edad_pac { get; set; }
        
        //DIRECCION PACIENTE
        
        [Required(ErrorMessage = "El campo cod_pais_dir_pac es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo cod_pais_dir_pac no debe exceder los 50 caracteres.")]
        public string cod_pais_dir_pac { get; set; }

        [Required(ErrorMessage = "El campo ubigeo_dir_pac es obligatorio")]
        [MaxLength(50, ErrorMessage = "El campo ubigeo_dir_pac no debe exceder los 50 caracteres.")]
        public string ubigeo_dir_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo cod_procede_pac no debe exceder los 50 caracteres.")]
        public string? cod_procede_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo tipo_via_pac no debe exceder los 50 caracteres.")]
        public string? tipo_via_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo num_puerta_pac no debe exceder los 50 caracteres.")]
        public string? num_puerta_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo agrup_rural_pac no debe exceder los 50 caracteres.")]
        public string? agrup_rural_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo nombre_agrup_pac no debe exceder los 50 caracteres.")]
        public string? nombre_agrup_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo num_manzana_pac no debe exceder los 50 caracteres.")]
        public string? num_manzana_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo num_block_pac no debe exceder los 50 caracteres.")]
        public string? num_block_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo num_interior_pac no debe exceder los 50 caracteres.")]
        public string? num_interior_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo num_kilometro_pac no debe exceder los 50 caracteres.")]
        public string? num_kilometro_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo num_lote_pac no debe exceder los 50 caracteres.")]
        public string? num_lote_pac { get; set; }

        [MaxLength(50, ErrorMessage = "El campo referencia_pac no debe exceder los 50 caracteres.")]
        public string? referencia_pac { get; set; }

        //LUGAR PROBABLE INFECCION
        [Required(ErrorMessage = "El campo cod_ubigeo_pac es obligatorio")]
        [MaxLength(10, ErrorMessage = "El campo cod_ubigeo_pac no debe exceder los 10 caracteres.")]
        public string cod_ubigeo_pac { get; set; }

        [MaxLength(10, ErrorMessage = "El campo cod_local_pac no debe exceder los 10 caracteres.")]
        public string? cod_local_pac { get; set; }

        //DIAGNOSTICO
        [Required(ErrorMessage = "El campo cod_diag es obligatorio")]
        [MaxLength(10, ErrorMessage = "El campo cod_diag no debe exceder los 10 caracteres.")]
        public string cod_diag { get; set; }

        [Required(ErrorMessage = "El campo cod_tipo_diag es obligatorio")]
        [MaxLength(1, ErrorMessage = "El campo cod_tipo_diag no debe ser mayor a 1 caracter.")]
        public string cod_tipo_diag { get; set; }

        [MaxLength(1, ErrorMessage = "El campo protegido_diag no debe ser mayor a 1 caracter.")]
        [RegularExpression("(?i)^(S|N|I)$", ErrorMessage = "El campo protegido_diag debe ser 'S' (Sí), 'N' (No) o 'I' (Ignorado).")]
        public string? protegido_diag { get; set; }

        [MaxLength(50, ErrorMessage = "El campo tipo_muestra_diag no debe exceder los 50 caracteres.")]
        [RegularExpression("(?i)^(S|N|I)$", ErrorMessage = "El campo tipo_muestra_diag debe ser 'S' (Sí), 'N' (No) o 'I' (Ignorado).")]
        public string? tipo_muestra_diag { get; set; }

        [MaxLength(1, ErrorMessage = "El campo gestante_diag no debe ser mayor a 1 caracter.")]
        [RegularExpression("(?i)^(S|N|I)$", ErrorMessage = "El campo gestante_diag debe ser 'S' (Sí), 'N' (No) o 'I' (Ignorado).")]
        public string? gestante_diag { get; set; }
        //FECHA Y SEMANAS EPIDEMIOLOGICAS
        [Required(ErrorMessage = "El campo fecha_ini_sint es obligatorio")]
        public DateOnly fecha_ini_sint { get; set; }
        public DateOnly? fecha_ini_def { get; set; }

        [MaxLength(1, ErrorMessage = "El campo cod_dengue_def no debe ser mayor a 1 caracter.")]
        [RegularExpression("(?i)^(S|N|I)$", ErrorMessage = "El campo cod_dengue_def debe ser 'S' (Sí), 'N' (No) o 'I' (En Investigación).")]
        public string? cod_dengue_def { get; set; }
        [Required(ErrorMessage = "El campo fecha_not es obligatorio")]
        public DateOnly fecha_not { get; set; }
        public string? codigo_cdc { get; set; }
        //DATOS DEL INS
        //ADICIONALES - DESPUES DE OBTENER RESULTADO DE EXAMENES
        public string? codigo_INS { get; set; }
        public string? codigoOrden { get; set; }
        public string? CodigoLoinc { get; set; }
        public string? Examen { get; set; }
        public string? FechaResultado { get; set; }
        public string? TipoMuestra { get; set; }
        public string? ListaResultado { get; set; }
        public int Estado { get; set; }
    }

    public class TestRequest
    {
        public string codigo_cdc { get; set; }
        public string codigo_ins { get; set; }
        public string codigoLoinc { get; set; }
        public string examen { get; set; }
        public string fechaResultado { get; set; }
        public string tipoMuestra { get; set; }
        public string listaResultado { get; set; }//objeto serializado
    }

    [TableName("LogTestCDC")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class LogTestCDC
    {
        public int Id { get; set; }
        public string Trama { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
