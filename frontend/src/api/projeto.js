import { chamarAPI } from './api';

async function apiCriarProjeto(codigoEquipe, nome)
{	
	let res = await chamarAPI('projeto', 'POST', null, {'codigoEquipe': codigoEquipe, 'nome': nome });
	
	if (!res.ok || !res.resposta)
		return null;

	return res.resposta;
}

async function apiTarefas(codigoProjeto)
{
	let res = await chamarAPI('projeto/tarefas', 'GET', {'codigoProjeto': codigoProjeto });
	
	if (!res.ok || !res.resposta)
		return null;

	return res.resposta;
}

async function apiObterProjeto(codigoProjeto)
{
	let res = await chamarAPI('projeto', 'GET', {'codigoProjeto': codigoProjeto });
	
	if (!res.ok || !res.resposta)
		return null;

	return res.resposta;
}

export { apiCriarProjeto, apiTarefas, apiObterProjeto }