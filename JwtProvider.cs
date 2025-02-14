public class JwtProvider : IJwtProvider
{
	//tuple to return 2 value
	public (string taken, int expireIn) GenerateTaken(ApplicationUser user)
	{
		//Generate claims that add in taken
		Claim[] claims = new Claim[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
			new Claim(JwtRegisteredClaimNames.Email, user.Email!),
			new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
			new Claim(JwtRegisteredClaimNames.FamilyName,user.LastName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		};
		//Generate Key to encyrept and decrypt taken
		var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OvQWhp2kSk9D1RG8rrHI1qLeiKmBxRaz"));
		//المسئول عن تشفير الايميل واللباس فبديله المفتاح اللي هيشفر بيه والاجورزم نوع التشفير
		var signingCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

		var expiresIn = 1800;
		var taken = new JwtSecurityToken(
			issuer: "SurveyBasketApp",//مين اللي عمل التوكن
			audience: "SurveyBasketUsers",// لمين التوكن دي
			claims: claims, //معلومات خاصه باليوزر صاحب هذه التوكن
			expires: DateTime.UtcNow.AddMinutes(expiresIn),
			signingCredentials:signingCredentials
		);

		return (taken: new JwtSecurityTokenHandler().WriteToken(taken), expireIn: expiresIn);
	}
}
