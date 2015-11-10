
		-- Scripts a serem inseridos --
		--							 --
		--							 --
		--							 --
		--							 --
		--							 --
-------------------------------------------------------------------------------------------------------------

--ALTER TABLE tb1611_Liberacoes_Proposta ADD prioridadeLIBERACAOPROPOSTA INT NULL


--GO


ALTER TABLE tb1602_Itens_Proposta ADD xmlsequenciaITEMPROPOSTA NTEXT NULL


GO


--NOVA TABELA PARA GERENCIAMENTO DE PROPOSTA NO PIKING MOBILE.
CREATE TABLE tb1651_Picking_Mobile
(
    codigoPICKINGMOBILE INT IDENTITY(1,1),
	propostaPICKINGMOBILE INT NOT NULL,
	usuarioPICKINGMOBILE INT NOT NULL,
	statusPICKINGMOBILE SMALLINT NOT NULL DEFAULT(0),
	horainicioPICKINGMOBILE DATETIME,
	horafimPICKINGMOBILE DATETIME
	CONSTRAINT PKpickingMobileID PRIMARY KEY (codigoPICKINGMOBILE)
	CONSTRAINT FKpropostaPicking FOREIGN KEY (propostaPICKINGMOBILE)
	REFERENCES tb1601_Propostas(codigoPROPOSTA)
)

GO

--ATUAL 
CREATE VIEW vwMobile_tb1601_Proposta

AS

SELECT COALESCE(codigoPICKINGMOBILE,0) AS codigoPICKINGMOBILE,
codigoPROPOSTA,numeroPROPOSTA, CONVERT(NVARCHAR, P.dataLIBERACAOPROPOSTA,103)  +' '+ CONVERT(NVARCHAR, P.dataLIBERACAOPROPOSTA,108) AS dataLIBERACAOPROPOSTA,clientePROPOSTA AS clientePROPOSTA, razaoEMPRESA ,
COALESCE(ordemseparacaoimpressaPROPOSTA,0) AS ordemseparacaoimpressaPROPOSTA,0 as volumesPROPOSTA
FROM tb1601_Propostas (NOLOCK) 
INNER JOIN tb1611_Liberacoes_Proposta P (NOLOCK) ON P.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
LEFT JOIN tb1611_Liberacoes_Proposta C (NOLOCK) ON C.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
LEFT JOIN tb0301_Empresas (NOLOCK) ON clientePROPOSTA = codigoEMPRESA
LEFT JOIN tb1651_Picking_Mobile ON propostaPICKINGMOBILE = codigoPROPOSTA AND statusPICKINGMOBILE = 0
WHERE statusPROPOSTA = 1 
AND P.liberacaoLIBERACAOPROPOSTA = 1 
AND C.liberacaoLIBERACAOPROPOSTA = 2 
AND P.liberadoLIBERACAOPROPOSTA = 1  
AND C.liberadoLIBERACAOPROPOSTA = 0
AND (codigoPROPOSTA NOT IN (
								SELECT propostaPICKINGMOBILE	
								FROM tb1651_Picking_Mobile 
								WHERE statusPICKINGMOBILE > 0
								
							))
OR codigoPROPOSTA IN (
						SELECT propostaPICKINGMOBILE	
						FROM tb1651_Picking_Mobile 
						WHERE statusPICKINGMOBILE = 0
								
					 )

GO

CREATE FUNCTION fn0003_informacoesProdutos ( @codigoProposta int )

	RETURNS @InformationTable TABLE
	   (
			codigoPRODUTO				INT,
			partnumberPRODUTO			NVARCHAR(50),
			nomePRODUTO					NVARCHAR(150),
			ean13PRODUTO				NVARCHAR(15),
			codigolotePRODUTO			INT,
			identificacaolotePRODUTO	NVARCHAR(50),
			codigolocalPRODUTO			INT,
			nomelocalPRODUTO			NVARCHAR(20)
	   )
	AS
	BEGIN
	   INSERT @InformationTable
		   SELECT codigoPRODUTO,partnumberPRODUTO,nomePRODUTO,ean13PRODUTO,codigoLOTE, identificacaoLOTE,codigoLOCAL,nomeLOCAL
					FROM tb1205_Lotes
					INNER JOIN tb0501_Produtos ON produtoLOTE = codigoPRODUTO
					INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON codigoLOTE = loteLOTELOCAL
															   AND  codigoLOTE IN  
																				(
																					SELECT loteRESERVA
																					FROM tb1206_Reservas (NOLOCK) 
																					INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
																					INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL
																					WHERE propostaITEMPROPOSTA = @codigoProposta
																					AND tipodocRESERVA = 1602 
																					AND statusITEMPROPOSTA = 3 
																					AND separadoITEMPROPOSTA = 0
																				)
					INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL 
					WHERE codigoPRODUTO IN (   
											SELECT produtoRESERVA AS codigoPRODUTO
											FROM tb1206_Reservas (NOLOCK) 
											INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
											WHERE propostaITEMPROPOSTA = @codigoProposta
											AND tipodocRESERVA = 1602 
											AND statusITEMPROPOSTA = 3 
				  							AND separadoITEMPROPOSTA = 0  
										)
			   RETURN
	END


-----Informações sobre cada produto existente na proposta informada
--CREATE FUNCTION fn0003_informacoesProdutos ( @codigoProposta int )

--RETURNS @InformationTable TABLE
--   (
--    codigoPRODUTO				INT,
--    partnumberPRODUTO			NVARCHAR(50),
--    nomePRODUTO					NVARCHAR(150),
--    ean13PRODUTO				NVARCHAR(15),
--    codigolotePRODUTO			INT,
--	identificacaolotePRODUTO	NVARCHAR(50),
--	codigolocalPRODUTO			INT,
--	nomelocalPRODUTO			NVARCHAR(20)
--   )
--AS
--BEGIN
--   INSERT @InformationTable
        
--			SELECT codigoPRODUTO,partnumberPRODUTO,nomePRODUTO,ean13PRODUTO,codigoLOTE, identificacaoLOTE,codigoLOCAL,nomeLOCAL
--			FROM tb1205_Lotes
--			INNER JOIN tb0501_Produtos ON produtoLOTE = codigoPRODUTO
--			INNER JOIN tb0301_Empresas ON codigoEMPRESA = empresaLOTE
--			INNER JOIN tb1207_Lotes_Armazens ON codigoLOTE = loteLOTEARMAZEM
--			INNER JOIN tb1203_Armazens ON armazemLOTEARMAZEM = codigoARMAZEM
--			INNER JOIN tb1201_Estoque ON produtoESTOQUE = produtoLOTEARMAZEM
--			INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON codigoLOTE = loteLOTELOCAL AND  codigoLOTE IN  
--																									(
--																									SELECT loteRESERVA
--																									FROM tb1206_Reservas (NOLOCK) 
--																									INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
--																									INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO
--																									INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL
--																									INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
--																									WHERE propostaITEMPROPOSTA = @codigoProposta
--																									AND tipodocRESERVA = 1602 
--																									AND statusITEMPROPOSTA = 3 
--																									AND separadoITEMPROPOSTA = 0
--																									)
--			INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL 
--			WHERE codigoPRODUTO IN (   
--									SELECT produtoRESERVA AS codigoPRODUTO
--									FROM tb1206_Reservas (NOLOCK) 
--									INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
--									INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO
--									INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL
--									INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
--									WHERE propostaITEMPROPOSTA = @codigoProposta
--									AND tipodocRESERVA = 1602 
--									AND statusITEMPROPOSTA = 3 
--				  					AND separadoITEMPROPOSTA = 0  
--								)
--   RETURN
--END

--GO


---REALIZA O SPLIT DE UM TEXTO E RETORNA UMA TABELA COM ESTAS INFORMAÇÕES
CREATE FUNCTION fn0003_SplitTitanium( @InputString VARCHAR(8000), @Delimiter VARCHAR(50))

RETURNS @Items TABLE (Item VARCHAR(8000))

AS
BEGIN
      IF @Delimiter = ' '
      BEGIN
            SET @Delimiter = ','
            SET @InputString = REPLACE(@InputString, ' ', @Delimiter)
      END

      IF (@Delimiter IS NULL OR @Delimiter = '')
            SET @Delimiter = ','

--INSERT INTO @Items VALUES (@Delimiter) -- Diagnostic
--INSERT INTO @Items VALUES (@InputString) -- Diagnostic

      DECLARE @Item           VARCHAR(8000)
      DECLARE @ItemList       VARCHAR(8000)
      DECLARE @DelimIndex     INT

      SET @ItemList = @InputString
      SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      WHILE (@DelimIndex != 0)
      BEGIN
            SET @Item = SUBSTRING(@ItemList, 0, @DelimIndex)
            INSERT INTO @Items VALUES (@Item)

            -- Set @ItemList = @ItemList minus one less item
            SET @ItemList = SUBSTRING(@ItemList, @DelimIndex+1, LEN(@ItemList)-@DelimIndex)
            SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      END -- End WHILE

      IF @Item IS NOT NULL -- At least one delimiter was encountered in @InputString
      BEGIN
            SET @Item = @ItemList
            INSERT INTO @Items VALUES (@Item)
      END

      -- No delimiters were encountered in @InputString, so just return @InputString
      ELSE INSERT INTO @Items VALUES (@InputString)

      RETURN

END



GO


--======RETORNA OS LOTES DOS PRODUTOS DE UMA PROPOSTA
CREATE FUNCTION fn1211_LotesReservaProduto(@codigoPRODUTO int,@propostaReserva int)

RETURNS NVARCHAR(20)

BEGIN
    
	DECLARE @NumLote NVARCHAR(20)
	DECLARE @NumsLotes NVARCHAR(20)

	SET @NumsLotes = ' '

	DECLARE Local_Cursor CURSOR FOR 


		SELECT loteRESERVA
		FROM tb1206_Reservas (NOLOCK)
		INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA
		INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO 
		WHERE produtoRESERVA = @codigoPRODUTO
		AND propostaITEMPROPOSTA = @propostaReserva
		AND tipodocRESERVA = 1602 
		AND statusITEMPROPOSTA = 3
		AND separadoITEMPROPOSTA = 0  
		ORDER BY produtoRESERVA ASC


    OPEN Local_Cursor

    FETCH NEXT FROM Local_Cursor INTO @NumLote

    WHILE @@FETCH_STATUS = 0
    BEGIN


        SET @NumsLotes =  @NumsLotes + ',' + @NumLote


        FETCH NEXT FROM Local_Cursor INTO @NumLote

    END

    CLOSE Local_Cursor
    DEALLOCATE Local_Cursor

RETURN SUBSTRING(LTRIM(RTRIM(@NumsLotes)),2,LEN(LTRIM(RTRIM(@NumsLotes)))-1)    

END 


GO

--=-=-=-=--=--==--=-==-=--==--==-
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---FUNCAO PARA RETORNAR OS LOCAIS DE ARMAZENAMENTO DO PRODUTO.
CREATE FUNCTION fn1211_LocaisLoteProduto(@codigoPRODUTO INT,@lotePRODUTO NVARCHAR(10))

RETURNS NVARCHAR(20)

BEGIN
    
	DECLARE @Local NVARCHAR(20)
	DECLARE @LocalNames NVARCHAR(20)

	SET @LocalNames = ' '

	DECLARE Local_Cursor CURSOR FOR 

	SELECT nomeLOCAL 
	FROM tb1205_Lotes
	INNER JOIN tb1212_Lotes_Locais ON codigoLOTE = loteLOTELOCAL
	INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
	WHERE produtoLOTE = @codigoPRODUTO AND codigoLOTE IN (SELECT * FROM  dbo.fn0003_SplitTitanium(@lotePRODUTO,',') )
	ORDER BY nomeLOCAL DESC

    OPEN Local_Cursor

    FETCH NEXT FROM Local_Cursor INTO @Local

    WHILE @@FETCH_STATUS = 0
    BEGIN


        SET @LocalNames =  @Local  + ',' + @LocalNames


        FETCH NEXT FROM Local_Cursor INTO @Local

    END

    CLOSE Local_Cursor
    DEALLOCATE Local_Cursor

RETURN SUBSTRING(LTRIM(RTRIM(@LocalNames)),1,LEN(LTRIM(RTRIM(@LocalNames)))-1)    

END 

GO


CREATE TABLE tb1652_Picking_Prioridade
(
   propostaPRIORIDADE	INT			NOT NULL,
   valorPRIORIDADE		INT			NOT NULL,
   usuarioPRIORIDADE	INT			NOT NULL,
   dataPRIORIDADE		DATETIME	NOT NULL
)


GO


CREATE PROCEDURE sps1601_manipulaPRIORIDADEPICKING(@TIPO_PROCEDIMENTO INT,@PROPOSTA INT,@USER INT )
   -----------------
   -- PARAMETRO @TIPO_PROCEDIMENTO
   -- || VALOR  || ACAO 
   --      1    || REALIZA O INSERT 
   --      2    || REALIZA O DELETE 
   --      3    || DECREMENTA A PRIORIDADE DO ITEM
   --      4    || INCREMENTA A PRIORIDADE DO ITEM
   --

AS 

BEGIN

	DECLARE @VALOR_ATUAL_PRIORIDADE AS INT
	DECLARE @PROPOSTA_PRIORIDADE_ANTERIOR AS INT 
	DECLARE @PROPOSTA_PRIORIDADE_POSTERIOR AS INT 

	 --REALIZA PROCEDIMENTOS PARA INSERIR A PROPOSTA NA TABELA DE PRIORIDADES
	IF(@TIPO_PROCEDIMENTO)=1
		BEGIN 
	    				-- VERIFICA SE A PROPOSTA A SER INSERIDA EXISTE NA TABELA
			IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0
			PRINT 'passei aqui'
				BEGIN 
				--RECUPERA O VALOR ATUAL DA DA PRIORIDADE
				SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
		    
				--DELETA A PROPOSTA DA TABELA
				DELETE 
				FROM tb1652_Picking_Prioridade 
				WHERE propostaPRIORIDADE = @PROPOSTA

				--ATUALIZA O VALOR DOS ITENS COM PRIORIDADE ACIMA DA PROPOSTA DELETADA
				UPDATE tb1652_Picking_Prioridade
				SET valorPRIORIDADE = valorPRIORIDADE-1
				WHERE valorPRIORIDADE >=  @VALOR_ATUAL_PRIORIDADE

			END

			INSERT INTO tb1652_Picking_Prioridade
			(propostaPRIORIDADE,dataPRIORIDADE,usuarioPRIORIDADE,valorPRIORIDADE)
			VALUES 
			(@PROPOSTA,GETDATE(),@USER, (SELECT COALESCE(MAX(valorPRIORIDADE),0)FROM tb1652_Picking_Prioridade) + 1 )

			SELECT * 
			FROM tb1652_Picking_Prioridade
			ORDER BY valorPRIORIDADE ASC

	END
	--REALIZA PROCEDIMENTOS PARA DELETAR A PROPOSTA NA TABELA DE PRIORIDADES
	IF(@TIPO_PROCEDIMENTO)=2
		  BEGIN 

			IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0 
				--RECUPERA O VALOR ATUAL DA PRIORIDADE
				SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
		    
				--DELETA A PROPOSTA DA TABELA
				DELETE 
				FROM tb1652_Picking_Prioridade 
				WHERE propostaPRIORIDADE = @PROPOSTA

				--ATUALIZA O VALOR DOS ITENS COM PRIORIDADE ACIMA DA PROPOSTA DELETADA
				UPDATE tb1652_Picking_Prioridade
				SET valorPRIORIDADE = valorPRIORIDADE-1
				WHERE valorPRIORIDADE >=  @VALOR_ATUAL_PRIORIDADE

				SELECT * 
				FROM tb1652_Picking_Prioridade
				ORDER BY valorPRIORIDADE ASC
				
            END
	--REALIZA DECREMENTO NA PRIORIDADE DA PROPOSTA TRATADA 
	IF(@TIPO_PROCEDIMENTO)=3
		BEGIN 

			IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0
					--RECUPERA O VALOR PRIORIDADE DA PROPOSTA A SER TRABALHADA
					SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
					print 'proposta a ser tratada ' + convert(varchar(10),@VALOR_ATUAL_PRIORIDADE)

					--CASO A PROPOSTA TENHA A PRIORIDADE MAIOR QUE UM REALIZA O PROCEDIMENTO
					IF(@VALOR_ATUAL_PRIORIDADE)>1
						BEGIN
						    --RECUPERA A PROPOSTA COM PRIORIDADE ANTERIOR
							SET @PROPOSTA_PRIORIDADE_ANTERIOR = (SELECT propostaPRIORIDADE FROM tb1652_Picking_Prioridade WHERE valorPRIORIDADE = (@VALOR_ATUAL_PRIORIDADE -1)  )
                            
							--INCREMENTA A PROPOSTA ANTERIOR
							UPDATE tb1652_Picking_Prioridade
							SET  valorPRIORIDADE = valorPRIORIDADE+1
								 ,dataPRIORIDADE = GETDATE()
								 ,usuarioPRIORIDADE = @USER
							WHERE propostaPRIORIDADE = @PROPOSTA_PRIORIDADE_ANTERIOR;

							--DECREMENTA A PROPOSTA TRATADA
							UPDATE tb1652_Picking_Prioridade
							SET valorPRIORIDADE = valorPRIORIDADE-1
								,dataPRIORIDADE = GETDATE()
								,usuarioPRIORIDADE = @USER
							WHERE propostaPRIORIDADE = @PROPOSTA;

						END

                SELECT * 
				FROM tb1652_Picking_Prioridade
				ORDER BY valorPRIORIDADE ASC

		END
    --REALIZA INCREMENTO NA PRIORIDADE DA PROPOSTA TRATADA 
	IF(@TIPO_PROCEDIMENTO)=4
		  BEGIN 

				IF( SELECT COUNT(*) FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA ) > 0
					--RECUPERA O VALOR PRIORIDADE DA PROPOSTA A SER TRABALHADA
					SET  @VALOR_ATUAL_PRIORIDADE = (SELECT valorPRIORIDADE FROM tb1652_Picking_Prioridade WHERE propostaPRIORIDADE = @PROPOSTA)
					print 'proposta a ser tratada ' + convert(varchar(10),@VALOR_ATUAL_PRIORIDADE)

					--CASO A PROPOSTA TENHA A PRIORIDADE MAIOR QUE UM REALIZA O PROCEDIMENTO
					IF(@VALOR_ATUAL_PRIORIDADE)>=1

						IF( @VALOR_ATUAL_PRIORIDADE < (SELECT MAX(valorPRIORIDADE)FROM tb1652_Picking_Prioridade ))
							BEGIN
								--RECUPERA A PROPOSTA COM PRIORIDADE ANTERIOR
								SET @PROPOSTA_PRIORIDADE_POSTERIOR = (SELECT propostaPRIORIDADE FROM tb1652_Picking_Prioridade WHERE valorPRIORIDADE = (@VALOR_ATUAL_PRIORIDADE + 1)  )
                            
								--DECREMENTA A PROPOSTA POSTERIOR
								UPDATE tb1652_Picking_Prioridade
								SET  valorPRIORIDADE = valorPRIORIDADE-1
									 ,dataPRIORIDADE = GETDATE()
									 ,usuarioPRIORIDADE = @USER
								WHERE propostaPRIORIDADE = @PROPOSTA_PRIORIDADE_POSTERIOR;

								--INCREMENTA A PROPOSTA TRATADA
								UPDATE tb1652_Picking_Prioridade
								SET valorPRIORIDADE = valorPRIORIDADE+1
									,dataPRIORIDADE = GETDATE()
									,usuarioPRIORIDADE = @USER
								WHERE propostaPRIORIDADE = @PROPOSTA;

							END

                SELECT * 
				FROM tb1652_Picking_Prioridade
				ORDER BY valorPRIORIDADE ASC

	END
	 
END























---------------------------              ---------------------------------------
-----------------          -----------------------------------------------------
---------------------------             ----------------------------------------
----------------            ----------------------------------------------------
----------------------                         ---------------------------------
--------------------------               ---------------------------------------




DROP TABLE tb0004_Etiquetas
DROP TABLE tb0003_Produtos
DROP TABLE tb0002_ItensProposta
DROP TABLE tb0001_Propostas


CREATE TABLE tb0001_Propostas 
(
	codigoPROPOSTA						INT NOT NULL CONSTRAINT PKPropostas PRIMARY KEY,
	numeroPROPOSTA						NVARCHAR(20) NOT NULL,
	dataliberacaoPROPOSTA				NVARCHAR(20) NOT NULL,
	clientePROPOSTA						INT NOT NULL,
	razaoclientePROPOSTA				NVARCHAR(200),
	volumesPROPOSTA              		SMALLINT,
	operadorPROPOSTA					INT,
	codigopickingmobilePROPOSTA         INT
)

GO 

CREATE TABLE tb0002_ItensProposta 
(
	codigoITEMPROPOSTA				INT NOT NULL CONSTRAINT PKItensProposta PRIMARY KEY,
	propostaITEMPROPOSTA			INT ,
	quantidadeITEMPROPOSTA			REAL,
	statusseparadoITEMPROPOSTA		SMALLINT,
	codigoprodutoITEMPROPOSTA		INT,
	lotereservaITEMPROPOSTA			INT,
	xmlSequenciaITEMPROPOSTA		NTEXT
)
GO

CREATE TABLE tb0003_Produtos (
	codigoPRODUTO				INT NOT NULL ,
	ean13PRODUTO				NVARCHAR(15) NOT NULL ,
	partnumberPRODUTO			NVARCHAR(100) ,
	descricaoPRODUTO			NVARCHAR(100) ,
	codigolotePRODUTO			INT,
	identificacaolotePRODUTO	NVARCHAR(100) ,
	codigolocalPRODUTO			INT , 
	nomelocalPRODUTO			NVARCHAR(100)   )

GO    

CREATE TABLE tb0004_Etiquetas 
(
	codigoETIQUETA				    INT IDENTITY(1,1) NOT NULL CONSTRAINT PKEtiquetas PRIMARY KEY,
	eanItempropostaETIQUETA			NVARCHAR(20) NOT NULL,
	volumeETIQUETA	      			INT NOT NULL,
	quantidadeETIQUETA	      		REAL NOT NULL,
	sequenciaETIQUETA    			INT NOT NULL
)

GO 
	CREATE TABLE tb0005_Embalagens (
	codigoEMBALAGEM				    INT				NOT NULL CONSTRAINT PKSequencia PRIMARY KEY,
	nomeEMBALAGEM         			NVARCHAR(20)		NULL,
	produtoEMBALAGEM	      		INT				NOT NULL,
	quantidadeEMBALAGEM	      		REAL			NOT NULL,
	padraoEMBALAGEM    			    SMALLINT		NOT NULL,
	embalagemEMBALAGEM    			INT				NOT NULL,
	ean13EMBALAGEM    			    NVARCHAR(13)	NOT NULL)




 --//CARGA DE TESTES

Insert INTO tb0001_Propostas VALUES (75514,'75514-1','20/05/2015 10:37:34',6191,'DIEGO ALEJANDRO RECZEK',0,114)

GO 

		-- INCLUSÃO CAMPOS EM TABELAS ---
ALTER TABLE tb1611_Liberacoes_Proposta ADD prioridadeLIBERACAOPROPOSTA INT NULL

ALTER TABLE tb1602_Itens_Proposta ADD xmlsequenciaITEMPROPOSTA NTEXT NULL

----INICIAL DESATIVADA
--CREATE VIEW vwMobile_tb1601_Proposta

--AS

--SELECT codigoPROPOSTA,numeroPROPOSTA, CONVERT(NVARCHAR, P.dataLIBERACAOPROPOSTA,103)  +' '+ CONVERT(NVARCHAR, P.dataLIBERACAOPROPOSTA,108) as dataLIBERACAOPROPOSTA,clientePROPOSTA AS clientePROPOSTA, razaoEMPRESA ,
--COALESCE(ordemseparacaoimpressaPROPOSTA,0) AS ordemseparacaoimpressaPROPOSTA, P.prioridadeLIBERACAOPROPOSTA AS Prioridade,0 as volumesPROPOSTA
--FROM tb1601_Propostas (NOLOCK) 
--INNER JOIN tb1611_Liberacoes_Proposta P (NOLOCK) ON P.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
--LEFT JOIN tb1611_Liberacoes_Proposta C (NOLOCK) ON C.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
--LEFT JOIN tb0301_Empresas (NOLOCK) ON clientePROPOSTA = codigoEMPRESA
--WHERE statusPROPOSTA = 1 
--AND P.liberacaoLIBERACAOPROPOSTA = 1 
--AND C.liberacaoLIBERACAOPROPOSTA = 2 
--AND P.liberadoLIBERACAOPROPOSTA = 1  
--AND C.liberadoLIBERACAOPROPOSTA = 0
--AND P.prioridadeLIBERACAOPROPOSTA >= 0
----ORDER BY  Prioridade ASC,dataLIBERACAOPROPOSTA ASC


--NOVA TABELA PARA GERENCIAMENTO DE PROPOSTA NO PIKING MOBILE.
CREATE TABLE tb1651_Picking_Mobile
(
    codigoPICKINGMOBILE int identity(1,1),
	propostaPICKINGMOBILE INT NOT NULL,
	usuarioPICKINGMOBILE INT NOT NULL,
	statusPICKINGMOBILE SMALLINT NOT NULL DEFAULT(0),
	horainicioPICKINGMOBILE DATETIME,
	horafimPICKINGMOBILE DATETIME
	CONSTRAINT PKpickingMobileID PRIMARY KEY (codigoPICKINGMOBILE)
	CONSTRAINT FKpropostaPicking FOREIGN KEY (propostaPICKINGMOBILE)
	REFERENCES tb1601_Propostas(codigoPROPOSTA)
)



--ATUAL 
CREATE VIEW vwMobile_tb1601_Proposta

AS

SELECT COALESCE(codigoPICKINGMOBILE,0) AS codigoPICKINGMOBILE,
codigoPROPOSTA,numeroPROPOSTA, CONVERT(nvarchar, P.dataLIBERACAOPROPOSTA,103)  +' '+ CONVERT(nvarchar, P.dataLIBERACAOPROPOSTA,108) as dataLIBERACAOPROPOSTA,clientePROPOSTA AS clientePROPOSTA, razaoEMPRESA ,
COALESCE(ordemseparacaoimpressaPROPOSTA,0) AS ordemseparacaoimpressaPROPOSTA, P.prioridadeLIBERACAOPROPOSTA AS Prioridade,0 as volumesPROPOSTA
FROM tb1601_Propostas (NOLOCK) 
INNER JOIN tb1611_Liberacoes_Proposta P (NOLOCK) ON P.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
LEFT JOIN tb1611_Liberacoes_Proposta C (NOLOCK) ON C.propostaLIBERACAOPROPOSTA = codigoPROPOSTA 
LEFT JOIN tb0301_Empresas (NOLOCK) ON clientePROPOSTA = codigoEMPRESA
LEFT JOIN tb1651_Picking_Mobile ON propostaPICKINGMOBILE = codigoPROPOSTA AND statusPICKINGMOBILE =0
WHERE statusPROPOSTA = 1 
AND P.liberacaoLIBERACAOPROPOSTA = 1 
AND C.liberacaoLIBERACAOPROPOSTA = 2 
AND P.liberadoLIBERACAOPROPOSTA = 1  
AND C.liberadoLIBERACAOPROPOSTA = 0
AND P.prioridadeLIBERACAOPROPOSTA >= 0
AND (codigoPROPOSTA NOT IN (
								SELECT propostaPICKINGMOBILE	
								FROM tb1651_Picking_Mobile 
								WHERE statusPICKINGMOBILE > 0
								
							))
OR codigoPROPOSTA IN (
						SELECT propostaPICKINGMOBILE	
						FROM tb1651_Picking_Mobile 
						WHERE statusPICKINGMOBILE = 0
								
					 )


AND P.dataLIBERACAOPROPOSTA IS NOT NULL
GROUP BY codigoPICKINGMOBILE,codigoPROPOSTA,numeroPROPOSTA, CONVERT(nvarchar, P.dataLIBERACAOPROPOSTA,103)  +' '+ CONVERT(nvarchar, P.dataLIBERACAOPROPOSTA,108)
,clientePROPOSTA , razaoEMPRESA ,COALESCE(ordemseparacaoimpressaPROPOSTA,0), P.prioridadeLIBERACAOPROPOSTA ,volumesPROPOSTA 






----NOA UTILIZO NO CODIO MOBILE
--########################################################################################################################################
--# Atualiza os registros sem vínculos para a embalagem padrão
--# Gabriel
--# dd/mm/yyyy
--########################################################################################################################################

IF ( SELECT COUNT(codigoEMBALAGEMPRODUTO)
	 FROM tb0504_Embalagens_Produtos
	 LEFT JOIN tb0545_Embalagens ON codigoEMBALAGEM = embalagemEMBALAGEMPRODUTO
	 WHERE embalagemEMBALAGEMPRODUTO IS NULL
    ) > 0

BEGIN

	 UPDATE tb0504_Embalagens_Produtos
	 SET embalagemEMBALAGEMPRODUTO = (SELECT codigoEMBALAGEM
			  FROM tb0545_Embalagens
			  WHERE nomeEMBALAGEM = 'Padrão'
			 )
	 WHERE embalagemEMBALAGEMPRODUTO IS NULL

END

GO

--INFORMAÇÔES SOBRE OS ITENS DA PROPOSTA
SELECT codigoITEMPROPOSTA,propostaITEMPROPOSTA,produtoRESERVA AS codigoPRODUTO,nomePRODUTO,partnumberPRODUTO,ean13PRODUTO,SUM(quantidadeRESERVA) AS QTD
,quantidadeEMBALAGEMPRODUTO AS QtdEmbalagem
,dbo.fn1211_LotesReservaProduto(produtoRESERVA,propostaITEMPROPOSTA) AS lotesRESERVA
,DBO.fn1211_LocaisLoteProduto(produtoRESERVA,dbo.fn1211_LotesReservaProduto(produtoRESERVA,propostaITEMPROPOSTA)) AS locaisLOTES
FROM tb1206_Reservas (NOLOCK)
--,LOTEreserva
INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA
INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO 
INNER JOIN tb0504_Embalagens_Produtos ON codigobarrasEMBALAGEMPRODUTO = ean13PRODUTO
LEFT JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL 
LEFT JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL 
WHERE propostaITEMPROPOSTA = 80471 
AND tipodocRESERVA = 1602 
AND statusITEMPROPOSTA = 3
AND separadoITEMPROPOSTA = 0  
GROUP BY codigoITEMPROPOSTA,propostaITEMPROPOSTA,ean13PRODUTO,produtoRESERVA,nomePRODUTO,partnumberPRODUTO
,quantidadeEMBALAGEMPRODUTO,codigobarrasEMBALAGEMPRODUTO
ORDER BY codigoPRODUTO



---Informações sobre cada produto exsitente na proposta informada
CREATE FUNCTION fn0003_informacoesProdutos ( @codigoProposta int )

RETURNS @InformationTable TABLE
   (
    codigoPRODUTO				INT,
    partnumberPRODUTO			NVARCHAR(50),
    nomePRODUTO					NVARCHAR(150),
    ean13PRODUTO				NVARCHAR(15),
    codigolotePRODUTO			INT,
	identificacaolotePRODUTO	NVARCHAR(50),
	codigolocalPRODUTO			INT,
	nomelocalPRODUTO			NVARCHAR(20)
   )
AS
BEGIN
   INSERT @InformationTable
        
			SELECT codigoPRODUTO,partnumberPRODUTO,nomePRODUTO,ean13PRODUTO,codigoLOTE, identificacaoLOTE,codigoLOCAL,nomeLOCAL
			FROM tb1205_Lotes
			INNER JOIN tb0501_Produtos ON produtoLOTE = codigoPRODUTO
			INNER JOIN tb0301_Empresas ON codigoEMPRESA = empresaLOTE
			INNER JOIN tb1207_Lotes_Armazens ON codigoLOTE = loteLOTEARMAZEM
			INNER JOIN tb1203_Armazens ON armazemLOTEARMAZEM = codigoARMAZEM
			INNER JOIN tb1201_Estoque ON produtoESTOQUE = produtoLOTEARMAZEM
			INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON codigoLOTE = loteLOTELOCAL AND  codigoLOTE IN  
																									(
																									SELECT loteRESERVA
																									FROM tb1206_Reservas (NOLOCK) 
																									INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
																									INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO
																									INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL
																									INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
																									WHERE propostaITEMPROPOSTA = @codigoProposta
																									AND tipodocRESERVA = 1602 
																									AND statusITEMPROPOSTA = 3 
																									AND separadoITEMPROPOSTA = 0
																									)
			INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL 
			WHERE codigoPRODUTO IN (   
									SELECT produtoRESERVA AS codigoPRODUTO
									FROM tb1206_Reservas (NOLOCK) 
									INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA 
									INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO
									INNER JOIN tb1212_Lotes_Locais (NOLOCK) ON loteRESERVA = loteLOTELOCAL
									INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
									WHERE propostaITEMPROPOSTA = @codigoProposta
									AND tipodocRESERVA = 1602 
									AND statusITEMPROPOSTA = 3 
				  					AND separadoITEMPROPOSTA = 0  
								)
   RETURN
END




---REALIZA O SPLIT DE UM TEXTO E RETORNA UMA TABELA COM ESTAS INFORMAÇÕES
CREATE FUNCTION SplitTitanium( @InputString VARCHAR(8000), @Delimiter VARCHAR(50))

RETURNS @Items TABLE (Item VARCHAR(8000))

AS
BEGIN
      IF @Delimiter = ' '
      BEGIN
            SET @Delimiter = ','
            SET @InputString = REPLACE(@InputString, ' ', @Delimiter)
      END

      IF (@Delimiter IS NULL OR @Delimiter = '')
            SET @Delimiter = ','

--INSERT INTO @Items VALUES (@Delimiter) -- Diagnostic
--INSERT INTO @Items VALUES (@InputString) -- Diagnostic

      DECLARE @Item                 VARCHAR(8000)
      DECLARE @ItemList       VARCHAR(8000)
      DECLARE @DelimIndex     INT

      SET @ItemList = @InputString
      SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      WHILE (@DelimIndex != 0)
      BEGIN
            SET @Item = SUBSTRING(@ItemList, 0, @DelimIndex)
            INSERT INTO @Items VALUES (@Item)

            -- Set @ItemList = @ItemList minus one less item
            SET @ItemList = SUBSTRING(@ItemList, @DelimIndex+1, LEN(@ItemList)-@DelimIndex)
            SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      END -- End WHILE

      IF @Item IS NOT NULL -- At least one delimiter was encountered in @InputString
      BEGIN
            SET @Item = @ItemList
            INSERT INTO @Items VALUES (@Item)
      END

      -- No delimiters were encountered in @InputString, so just return @InputString
      ELSE INSERT INTO @Items VALUES (@InputString)

      RETURN

END
GO


--------TAMBÈM REALIZA UM SPLIT MAS NAO ESTA EM USO.
--CREATE FUNCTION fn1211_SplitTitanium( @frase VARCHAR(max), @delimitador VARCHAR(max) = ',') 
--RETURNS @result TABLE (item VARCHAR(8000)) 

--BEGIN

--	DECLARE @parte VARCHAR(8000)

--	WHILE CHARINDEX(@delimitador,@frase,0) <> 0

--		BEGIN

--			SELECT
--			  @parte=RTRIM(LTRIM(
--					  SUBSTRING(@frase,1,
--					CHARINDEX(@delimitador,@frase,0)-1))),
--			  @frase=RTRIM(LTRIM(SUBSTRING(@frase,
--					  CHARINDEX(@delimitador,@frase,0)
--					+ LEN(@delimitador), LEN(@frase))))
--			IF LEN(@parte) > 0
--			  INSERT INTO @result SELECT @parte

--		END 

--		IF LEN(@frase) > 0
--			INSERT INTO @result SELECT @frase

--	RETURN

--END
--GO


--======RETORNA OS LOTES DOS PRODUTOS DE UMA PROPOSTA
CREATE FUNCTION fn1211_LotesReservaProduto(@codigoPRODUTO int,@propostaReserva int)

RETURNS NVARCHAR(20)

BEGIN
    
	DECLARE @NumLote NVARCHAR(20)
	DECLARE @NumsLotes NVARCHAR(20)

	SET @NumsLotes = ' '

	DECLARE Local_Cursor CURSOR FOR 


		SELECT loteRESERVA
		FROM tb1206_Reservas (NOLOCK)
		INNER JOIN tb1602_Itens_Proposta (NOLOCK) ON codigoITEMPROPOSTA = docRESERVA
		INNER JOIN tb0501_Produtos (NOLOCK) ON produtoITEMPROPOSTA = codigoPRODUTO 
		WHERE produtoRESERVA = @codigoPRODUTO
		AND propostaITEMPROPOSTA = @propostaReserva
		AND tipodocRESERVA = 1602 
		AND statusITEMPROPOSTA = 3
		AND separadoITEMPROPOSTA = 0  
		ORDER BY produtoRESERVA ASC


    OPEN Local_Cursor

    FETCH NEXT FROM Local_Cursor INTO @NumLote

    WHILE @@FETCH_STATUS = 0
    BEGIN


        SET @NumsLotes =  @NumsLotes + ',' + @NumLote


        FETCH NEXT FROM Local_Cursor INTO @NumLote

    END

    CLOSE Local_Cursor
    DEALLOCATE Local_Cursor

RETURN SUBSTRING(LTRIM(RTRIM(@NumsLotes)),2,LEN(LTRIM(RTRIM(@NumsLotes)))-1)    

END 




--=-=-=-=--=--==--=-==-=--==--==-
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---FUNCAO PARA RETORNAR OS LOCAIS DE ARMAZENAMENTO DO PRODUTO.
ALTER FUNCTION fn1211_LocaisLoteProduto(@codigoPRODUTO INT,@lotePRODUTO NVARCHAR(10))

RETURNS NVARCHAR(20)

BEGIN
    
	DECLARE @Local NVARCHAR(20)
	DECLARE @LocalNames NVARCHAR(20)

	SET @LocalNames = ' '

	DECLARE Local_Cursor CURSOR FOR 

	SELECT nomeLOCAL 
	FROM tb1205_Lotes
	INNER JOIN tb1212_Lotes_Locais ON codigoLOTE = loteLOTELOCAL
	INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
	WHERE produtoLOTE = @codigoPRODUTO AND codigoLOTE IN (SELECT * FROM  dbo.fn0003_SplitTitanium(@lotePRODUTO,',') )
	ORDER BY nomeLOCAL DESC

    OPEN Local_Cursor

    FETCH NEXT FROM Local_Cursor INTO @Local

    WHILE @@FETCH_STATUS = 0
    BEGIN


        SET @LocalNames =  @Local  + ',' + @LocalNames


        FETCH NEXT FROM Local_Cursor INTO @Local

    END

    CLOSE Local_Cursor
    DEALLOCATE Local_Cursor

RETURN SUBSTRING(LTRIM(RTRIM(@LocalNames)),1,LEN(LTRIM(RTRIM(@LocalNames)))-1)    

END 



---FUNCAO PARA RETORNAR OS LOCAIS DE ARMAZENAMENTO DO PRODUTO.
--CREATE FUNCTION fn1211_LocaisLoteProduto(@codigoPRODUTO int,@lotePRODUTO int)

--RETURNS NVARCHAR(20)

--BEGIN
    
--	DECLARE @Local NVARCHAR(20)
--	DECLARE @LocalNames NVARCHAR(20)

--	SET @LocalNames = ' '

--	DECLARE Local_Cursor CURSOR FOR 

--	SELECT nomeLOCAL 
--	FROM tb1205_Lotes
--	INNER JOIN tb1212_Lotes_Locais ON codigoLOTE = loteLOTELOCAL
--	INNER JOIN tb1211_Locais ON codigoLOCAL = localLOTELOCAL
--	WHERE produtoLOTE = @codigoPRODUTO AND codigoLOTE = @lotePRODUTO 
--	ORDER BY nomeLOCAL ASC

--    OPEN Local_Cursor

--    FETCH NEXT FROM Local_Cursor INTO @Local

--    WHILE @@FETCH_STATUS = 0
--    BEGIN


--        SET @LocalNames =  @Local  + ',' + @LocalNames


--        FETCH NEXT FROM Local_Cursor INTO @Local

--    END

--    CLOSE Local_Cursor
--    DEALLOCATE Local_Cursor

--RETURN SUBSTRING(LTRIM(RTRIM(@LocalNames)),1,LEN(LTRIM(RTRIM(@LocalNames)))-1)    

--END 



