using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSMS.Models
{
    public class DocumentUpload
    {
        [Required(ErrorMessage = "Please select a file.")]
        public List<IFormFile> Files { get; set; }

        public List<string> UploadedFiles { get; set; } = new List<string>();

        public List<string> Folders { get; set; } = new List<string>();

        public string CurrentPath { get; set; }
    }
}