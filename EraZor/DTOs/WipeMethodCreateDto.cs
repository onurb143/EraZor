using System.ComponentModel.DataAnnotations;

namespace EraZor.DTO
{
    public class WipeMethodCreateDto
    {
        /// Navnet på slette-metoden (f.eks. "Secure Erase").
        public string Name { get; set; }

        /// Antallet af overskrivningspasser.

        [Required(ErrorMessage = "OverWritePass is required.")]
        public int OverwritePass { get; set; }

        /// Beskrivelse af slette-metoden.
        public string Description { get; set; }
    }
}
