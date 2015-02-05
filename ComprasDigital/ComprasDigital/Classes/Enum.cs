using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComprasDigital.Classes
{
	public enum Ocorrencia
	{
		CodigoJaExistente = 1,
		CodigoDiferente = 2,
		TipoDiferente = 3,
		UnidadeDiferente = 4
	};

	public enum Embalagem
	{
		Outra = 1,
		Unidade = 2,
		Pacote = 3,
		Caixa = 4,
		Garrafa = 5,
		Lata = 6,
		Barra = 7,
		Peso = 8,
		Granel = 9
	};

	public enum Unidade
	{
		Unidade = 1,
		KG = 2,
		Gramas = 3,
		Litro = 4,
		ML = 5,
		Granel = 6
	};
}