SELECT 
      SUM(S.Cnt * P.Price), 2) AS TotalMoney
FROM 
      Sales S 
      JOIN Products P ON S.ID_product = P.Id
WHERE 
      CAST(S.Moment AS DATE ) = '2022-01-19'
--
