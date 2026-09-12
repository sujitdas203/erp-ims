
namespace IMS.Models.Common.Dropdown
{
    /// <summary>
    /// Generic dropdown item returned by the dropdown service.
    /// Works for all dropdowns in the application.
    /// </summary>
    public class DropdownItemModel
    {
        /// <summary>
        /// Dropdown Value.
        /// Usually the Primary Key.
        /// Example (INT): 1, 2, 10
        /// Example (GUID): "22222222-2222-2222-2222-222222222201"
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Dropdown Display Text.
        /// Example:
        /// Cash
        /// Electronics
        /// Samsung
        /// Piece
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Optional code.
        /// Example:
        /// CASH
        /// ELE001
        /// SAM
        /// PCS
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Parent Id.
        /// Used for cascading dropdowns.
        /// Example (INT): 1, 5
        /// Example (GUID): "22222222-2222-2222-2222-222222222201"
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// Record Active Status.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
