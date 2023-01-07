CREATE PROCEDURE sp_GravaProcessamentoFolha
@idDepartamento int,
@mesVigencia varchar(15),
@totalPagamentos decimal,
@totalDescontos decimal,
@totalExtras decimal,
@new_identity int = null OUTPUT

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO		ProcessamentoFolha
					(Departamento_idDepartamento, MesVigencia, TotalPagamentos, TotalDescontos, TotalExtras)
         SELECT		@idDepartamento, @mesVigencia, @totalPagamentos, @totalDescontos, @totalExtras;
		 SET @new_identity = SCOPE_IDENTITY();
END
GO