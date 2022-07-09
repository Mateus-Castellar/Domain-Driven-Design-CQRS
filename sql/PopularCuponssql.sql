insert into Cupons(Id, Codigo, Percentual, ValorDesconto, Quantidade, TipoDescontoCupom, DataCriacao, DataUtilizacao, DataValidade, Ativo, Utilizado)
values (NEWID(), 'PROMO-15-REAIS', NULL, 15, 0, 1, GETDATE(), null, GETDATE() + 1, 1, 0)

insert into Cupons(Id, Codigo, Percentual, ValorDesconto, Quantidade, TipoDescontoCupom, DataCriacao, DataUtilizacao, DataValidade, Ativo, Utilizado)
values (NEWID(), 'PROMO-10-OFF', 10, null, 50, 0, GETDATE(), null, GETDATE() + 90, 1, 0)