CREATE PROCEDURE sp_GravaDepartamento
@nomeDepartamento varchar(80)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO	Departamento
			(NomeDepartamento)
         SELECT		@nomeDepartamento;
	
	RETURN SCOPE_IDENTITY();
END
GO
