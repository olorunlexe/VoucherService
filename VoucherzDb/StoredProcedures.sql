USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_CreateDiscountVoucher]    Script Date: 1/16/2019 3:01:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_CreateDiscountVoucher]

   @HashedCode nvarchar(100),
   @voucherType nvarchar(50)= 'Discount',
   @MerchantId nvarchar(100),
   @DiscountAmount bigint = NULL,
   @DiscountPercent float = NULL,
   @DiscountUnit bigint = NULL,
   @ExpiryDate datetime


AS

Declare @voucherId bigint

   BEGIN TRY
    BEGIN TRANSACTION CreateDiscountVoucher

            
           INSERT INTO Voucher
               (
               [Code],[VoucherType],
               [MerchantId],
               [ExpiryDate]
               )
           VALUES(@HashedCode, @voucherType, @MerchantId, @ExpiryDate)

           SET @voucherId = SCOPE_IDENTITY()

           INSERT INTO DiscountVoucher
               ( -- columns to insert data into
               [DiscountAmount], [DiscountPercentage], [DiscountUnit],[VoucherId]
               )
           VALUES
               ( -- values for the columns in the list above
                   @DiscountAmount, @DiscountPercent, @DiscountUnit, @voucherId
               )
        COMMIT TRANSACTION CreateDiscountVoucher

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_CreateGiftVoucher]    Script Date: 1/16/2019 3:02:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_CreateGiftVoucher]
   @HashedCode NVARCHAR(100),
   @VoucherType NVARCHAR(50) = 'Gift',
   @ExpiryDate DATETIME,
   @MerchantId NVARCHAR(100),
   @GiftAmount BIGINT
AS
   -- body of the stored procedure
   DECLARE @VoucherId BIGINT

   BEGIN TRY
       BEGIN TRANSACTION
           -- Insert rows into table 'Voucherr'
           INSERT INTO Voucher
           ( -- columns to insert data into
           Code, VoucherType, ExpiryDate, MerchantId
           )
           VALUES
           ( -- first row: values for the columns in the list above
           @HashedCode, @VoucherType, @ExpiryDate, @MerchantId
           )

           --capture the current voucher id-------
           SET @VoucherId = SCOPE_IDENTITY()

           -- Insert rows into table 'GiftVoucher'
           INSERT INTO GiftVoucher
           (
           GiftAmount, GiftBalance, VoucherId
           )
           VALUES
           (
           @GiftAmount, @GiftAmount, @VoucherId
           )
       COMMIT TRANSACTION
   END TRY
   BEGIN CATCH
       ROLLBACK
   END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_CreateValueVoucher]    Script Date: 1/16/2019 3:02:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_CreateValueVoucher]
	@HashedCode NVARCHAR(100),
	@VoucherType NVARCHAR(50) = 'Value',
	@ExpiryDate DATETIME,
	@MerchantId NVARCHAR(100),
	@ValueAmount BIGINT
AS

	DECLARE
	@VId BIGINT
	BEGIN TRY
		BEGIN TRANSACTION 

		-- body of the stored procedure
		-- Insert rows into table 'Voucher'
		INSERT INTO Voucher
		( -- columns to insert data into
	 	 Code, VoucherType, ExpiryDate, MerchantId
		)
		VALUES
		( -- first row: values for the columns in the list above
	 	 @HashedCode, @VoucherType, @ExpiryDate, @MerchantId
		)

		SET @VId = SCOPE_IDENTITY()

		-- Insert rows into table 'ValueVoucher'
		INSERT INTO ValueVoucher
		( -- columns to insert data into
	 	 ValueAmount, VoucherId
		)
		VALUES
		( -- first row: values for the columns in the list above
	 	 @ValueAmount, @VId
		)
	COMMIT TRANSACTION
END TRY
BEGIN CATCH	
	ROLLBACK
END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_DeleteVoucherByCode]    Script Date: 1/16/2019 3:03:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_DeleteVoucherByCode]
   @Code nvarchar(100)


AS
   BEGIN TRY
		BEGIN TRANSACTION DeleteVoucherByCode

				UPDATE Voucher
				SET
					[VoucherStatus] = 'DELETED'
					-- add more columns and values here
				WHERE @Code=Code
			 
		COMMIT TRANSACTION DeleteVoucherByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_DeleteVoucherById]    Script Date: 1/16/2019 3:04:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_DeleteVoucherById]
   @VoucherId BIGINT


AS
   BEGIN TRY
		BEGIN TRANSACTION DeleteVoucherById

				UPDATE Voucher
				SET
					[VoucherStatus] = 'DELETED'
					-- add more columns and values here
				WHERE @VoucherId=VoucherId
			 
		COMMIT TRANSACTION DeleteVoucherById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllDiscountVouchers]    Script Date: 1/16/2019 3:04:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllDiscountVouchers] 
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
FROM Voucher_DiscountView

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllDiscountVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:52:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllDiscountVouchersFilterByMerchantId] 
@MercahntId BIGINT

AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
FROM Voucher_DiscountView
WHERE @MercahntId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllGiftVouchers]    Script Date: 1/16/2019 3:05:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllGiftVouchers]
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance
FROM Voucher_GiftView

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllGiftVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:54:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllGiftVouchersFilterByMerchantId]
@MerchantId BIGINT

AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance
FROM Voucher_GiftView
WHERE @MerchantId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllValueVouchers]    Script Date: 1/16/2019 3:05:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllValueVouchers]

AS

SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount

FROM Voucher_ValueView

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllValueVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:55:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllValueVouchersFilterByMerchantId]
@MerchantId BIGINT

AS

SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount

FROM Voucher_ValueView
WHERE @MerchantId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllVouchers]    Script Date: 1/16/2019 3:06:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllVouchers] 
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus
FROM Voucher

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetAllVouchersFilterByMerchantId]    Script Date: 1/16/2019 3:55:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetAllVouchersFilterByMerchantId]
@MerchantId BIGINT
 
AS
SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus
FROM Voucher
WHERE @MerchantId = MerchantId

GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCode]    Script Date: 1/16/2019 3:07:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByCode]
	@Code NVARCHAR(100) = NULL,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE Code LIKE '%'+@Code+'%'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE Code LIKE '%'+@Code+'%'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE Code LIKE '%'+@Code+'%'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE Code LIKE '%'+@Code+'%'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCodeFilterByMerchantId]    Script Date: 1/16/2019 3:07:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp_GetVoucherByCodeFilterByMerchantId]
	@Code NVARCHAR(100) = NULL,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE Code LIKE '%'+@Code+'%' AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCreationDate]    Script Date: 1/16/2019 3:08:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByCreationDate]
	@CreationDate DATETIME = NULL,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate)
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByCreationDateFilterByMerchantId]    Script Date: 1/16/2019 3:08:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp_GetVoucherByCreationDateFilterByMerchantId]
	@CreationDate DATETIME = NULL,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @CreationDate) LIKE CONVERT(date, CreationDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByExpiryDate]    Script Date: 1/16/2019 3:09:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByExpiryDate]
	@ExpiryDate DATETIME,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate)
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByExpiryDateFilterByMerchantId]    Script Date: 1/16/2019 3:09:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp_GetVoucherByExpiryDateFilterByMerchantId]
	@ExpiryDate DATETIME,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	CONVERT(date, @ExpiryDate) LIKE CONVERT(date, ExpiryDate) AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherById]    Script Date: 1/16/2019 3:09:56 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetVoucherById]
	@VoucherId BIGINT,
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherId = VoucherId
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE @VoucherId = VoucherId
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE @VoucherId = VoucherId
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE @VoucherId = VoucherId
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByIdFilterByMerchantId]    Script Date: 1/16/2019 3:10:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp_GetVoucherByIdFilterByMerchantId]
	@VoucherId BIGINT,
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE @VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE @VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE @VoucherId = VoucherId AND @MerchantId = MerchantId AND VoucherStatus <> 'DELETED'
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByMerchantId]    Script Date: 1/16/2019 3:10:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByMerchantId]
	@MerchantId NVARCHAR(100),
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	@MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	@MerchantId = MerchantId
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	@MerchantId = MerchantId
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByStatus]    Script Date: 1/16/2019 3:11:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetVoucherByStatus]
	@VoucherStatus NVARCHAR(10),
	@VoucherType NVARCHAR(50) = NULL

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherStatus = VoucherStatus
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	@VoucherStatus = VoucherStatus
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	@VoucherStatus = VoucherStatus
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	@VoucherStatus = VoucherStatus
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_GetVoucherByStatusFilterByMerchantId]    Script Date: 1/16/2019 3:12:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetVoucherByStatusFilterByMerchantId]
	@VoucherStatus NVARCHAR(10),
	@VoucherType NVARCHAR(50) = NULL,
	@MerchantId NVARCHAR(100)

	AS
	BEGIN TRY
		BEGIN TRANSACTION 

			IF @VoucherType = 'Value'

				BEGIN
					-- body of the stored procedure
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId,  MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, ValueAmount, VoucherId
					FROM dbo.Voucher_ValueView
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Discount'
		
				BEGIN
					-- Select rows from a Table or View 'Voucher_ValueView' in schema 'dbo'
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, VoucherStatus, ExpiryDate, DiscountAmount, DiscountPercentage, DiscountUnit, RedemptionCount
					FROM dbo.Voucher_DiscountView
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END

			ELSE IF @VoucherType = 'Gift' 
	
				BEGIN
					SELECT VoucherId, MerchantId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, GiftAmount, GiftBalance 
					FROM dbo.Voucher_GiftView
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END

			ELSE

				BEGIN 
					SELECT  VoucherId, Code, VoucherType, CreationDate, ExpiryDate, VoucherStatus, MerchantId
					FROM dbo.Voucher
					WHERE 	@VoucherStatus = VoucherStatus AND @MerchantId = MerchantId
				END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK
	END CATCH
GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateGiftAmountByCode]    Script Date: 1/16/2019 3:13:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_UpdateGiftAmountByCode]
   @Code nvarchar(100),
   @GiftAmount BIGINT


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateGiftAmountByCode
 
				-- Update rows in table 'TableName'
				UPDATE [dbo].[Voucher_GiftView]
				SET
					[GiftAmount] = @GiftAmount, GiftBalance = GiftBalance + @GiftAmount
					-- add more columns and values here
				WHERE @Code=Code AND @GiftAmount > GiftAmount
			 
		COMMIT TRANSACTION UpdateGiftAmountByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateGiftAmountById]    Script Date: 1/16/2019 3:13:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_UpdateGiftAmountById]
   @VoucherId BIGINT,
   @GiftAmount BIGINT


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateGiftAmountById
 
				-- Update rows in table 'TableName'
				UPDATE [dbo].[Voucher_GiftView]
				SET
					[GiftAmount] = @GiftAmount, GiftBalance = GiftBalance + @GiftAmount
					-- add more columns and values here
				WHERE @VoucherId=VoucherId AND @GiftAmount > GiftAmount
			 
		COMMIT TRANSACTION UpdateGiftAmountById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherExpiryDateByCode]    Script Date: 1/16/2019 3:14:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_UpdateVoucherExpiryDateByCode]
   @Code nvarchar(100),
   @ExpiryDate DATETIME


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherExpiryDateByCode
 
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[ExpiryDate] =@ExpiryDate
					-- add more columns and values here
				WHERE @Code=Code AND VoucherStatus <> 'DELETED'

		COMMIT TRANSACTION UpdateVoucherExpiryDateByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherExpiryDateById]    Script Date: 1/16/2019 3:14:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_UpdateVoucherExpiryDateById]
   @VoucherId nvarchar(100),
   @ExpiryDate DATETIME


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherExpiryDateById
 
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[ExpiryDate] =@ExpiryDate
					-- add more columns and values here
				WHERE @VoucherId=VoucherId AND VoucherStatus <> 'DELETED'

		COMMIT TRANSACTION UpdateVoucherExpiryDateById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherStatusByCode]    Script Date: 1/16/2019 3:15:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_UpdateVoucherStatusByCode]
   @Code nvarchar(100),
   @VoucherStatus VARCHAR(8)


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherStatusByCode

			IF @VoucherStatus = 'ACTIVE'  
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE @Code=Code

			ELSE IF @VoucherStatus = 'INACTIVE'
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE @Code=Code
			 
		COMMIT TRANSACTION UpdateVoucherStatusByCode

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------

USE [Voucherz]
GO

/****** Object:  StoredProcedure [dbo].[usp_UpdateVoucherStatusById]    Script Date: 1/16/2019 3:15:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp_UpdateVoucherStatusById]
   @VoucherId nvarchar(100),
   @VoucherStatus VARCHAR(8)


AS
   BEGIN TRY
		BEGIN TRANSACTION UpdateVoucherStatusById

			IF @VoucherStatus = 'ACTIVE'  
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE VoucherId=@VoucherId

			ELSE IF @VoucherStatus = 'INACTIVE'
				-- Update rows in table 'TableName'
				UPDATE Voucher
				SET
					[VoucherStatus] =@VoucherStatus
					-- add more columns and values here
				WHERE VoucherId=@VoucherId

		COMMIT TRANSACTION UpdateVoucherStatusById

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
    END CATCH


GO

-----------------------------------------------------------------------------------------------------------------------------



















