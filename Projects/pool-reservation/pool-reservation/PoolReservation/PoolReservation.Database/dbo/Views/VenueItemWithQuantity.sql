CREATE VIEW [dbo].[VenueItemWithQuantity]
	AS
	SELECT TOP 1 vi.*, vq.Quantity FROM [VenueItems] vi, [VenueItemQuantity] vq
	WHERE vi.Id = vq.ItemId
	AND vq.DateEffective <= CONVERT(date, GETUTCDATE())  
	ORDER BY  vq.DateEffective DESC
	


--CREATE VIEW [dbo].[VenueItemWithQuantity]
--	AS
--	SELECT TOP 1 vi.*, vq.Quantity FROM [VenueItems] vi
--	LEFT JOIN [VenueItemQuantity] vq ON vq.ItemId = vi.Id
--	AND vq.DateEffective <= CONVERT(date, GETUTCDATE())  
--	ORDER BY vq.DateEffective DESC
