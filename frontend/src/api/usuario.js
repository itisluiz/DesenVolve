import { chamarAPI } from './api';

async function login(email, senha)
{	
	let res = await chamarAPI('login', 'POST', null, {'email': email, 'senha': senha});
	
	if (!res.ok || !res.resposta)
		return null;

	return res.resposta;
}

async function cadastro(nome, sobrenome, email, senha)
{
	let res = await chamarAPI('usuario', 'POST', null, {'nome': nome, 'sobrenome': sobrenome, 'email': email, 'senha': senha});
	
	if (!res.ok || !res.resposta)
		return null;

	return res.resposta;
}

async function logout()
{
	await chamarAPI('logout', 'POST');
}


async function logado()
{
	let res = await chamarAPI('usuario', 'GET');
	
	if (!res.ok || !res.resposta)
		return null;

	return res.resposta;
}

export { login, cadastro, logout, logado }
