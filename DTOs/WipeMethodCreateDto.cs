namespace EraZor.DTO
{
    public class WipeMethodCreateDto
    {
        /// Navnet på slette-metoden (f.eks. "Secure Erase").
        public required string Name { get; set; }

        /// Antallet af overskrivningspasser.
        public int OverwritePass { get; set; }

        /// Beskrivelse af slette-metoden.
        public required string Description { get; set; }
    }
}
