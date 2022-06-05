namespace M13.InterviewProject.Models.Form
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    public class AddRuleForm
    {
        [Required]
        [NotNull]
        public string Site { get; set; }
        [Required]
        [NotNull]
        public string Value { get; set; }

        public override string ToString() => $"{Site}: {Value}";
    }
}
