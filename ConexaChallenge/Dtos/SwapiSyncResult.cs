namespace ConexaChallenge.Dtos
{
    public class SwapiSyncResult
    {
        public int Created { get; set; }
        public int Updated { get; set; }
        public int Total => Created + Updated;

    }
}
