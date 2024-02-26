using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserCardLike
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[ForeignKey("User")]
	public string User_Id { get; set; }
	public UserSqlModel User { get; set; }

	[ForeignKey("Card")]
	public string Card_Id { get; set; }
	public CardSqlModel Card { get; set; }
}
