CREATE PROCEDURE sp_GravaProcessamentoFolha
@idDepartamento int,
@mesVigencia varchar(15),
@totalPagamentos decimal(7,2),
@totalDescontos decimal(7,2),
@totalExtras decimal(7,2),
@anoVigencia varchar(4)


AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO	ProcessamentoFolha
			(Departamento_idDepartamento, MesVigencia, TotalPagamentos, TotalDescontos, TotalExtras, AnoVigencia)
         SELECT		@idDepartamento, @mesVigencia, @totalPagamentos, @totalDescontos, @totalExtras, @anoVigencia;
	
	RETURN SCOPE_IDENTITY();
END
GO
