namespace market_place
{
    /// <summary>
    /// Perfil de un agricultor: identidad de la finca, estado de verificación e información de pago.
    /// Mapea a la tabla "farmer_profiles".
    /// </summary>
    public class FarmerProfile
    {
        /// <summary>Identificador único del perfil (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>Nombre de la finca o del agricultor.</summary>
        public string farmName { get; set; } = string.Empty;

        /// <summary>Descripción de la finca y sus productos.</summary>
        public string description { get; set; } = string.Empty;

        /// <summary>Estado de verificación del perfil (Pending, Approved, Rejected).</summary>
        public VerificationStatus verificationStatus { get; set; }

        /// <summary>Información de la cuenta bancaria para pagos (opcional).</summary>
        public string bankAccountInfo { get; set; } = string.Empty;

        /// <summary>Fecha de creación del perfil en formato "yyyy-MM-dd HH:mm:ss".</summary>
        public DateTime createdAt { get; set; }
    }
}
