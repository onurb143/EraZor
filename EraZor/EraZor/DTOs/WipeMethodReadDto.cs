namespace EraZor.DTO
{
    public class WipeMethodReadDto
    {
        /// Unik identifikator for slette-metoden.
        public int WipeMethodID { get; set; }

        /// Navnet på slette-metoden (f.eks. "Secure Erase").
        public required string Name { get; set; }

        /// Antallet af overskrivningspasser.
        public int OverwritePass { get; set; }

        /// Beskrivelse af slette-metoden.
        public required string Description { get; set; }
    }
}
