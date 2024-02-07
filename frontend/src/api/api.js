import { executarAcao } from "./autonomia";

async function chamarAPI(rota, verbo, argsQuery = null, argsForm = null, autonomia = true)
{
	rota = import.meta.env.VITE_URL_API + `${rota}${argsQuery ? `?${new URLSearchParams(argsQuery)}` : '/' }`;
	verbo = verbo.trim().toUpperCase();

	let corpo = null;
	let cabecalhos = {};

	if (verbo != 'GET' && argsForm) {
		cabecalhos['Content-Type'] = 'application/x-www-form-urlencoded';
		corpo = new URLSearchParams(argsForm);
	}

	let res = await fetch(rota, {
		method: verbo,
		headers: cabecalhos,
		body: corpo,
		mode: import.meta.env.DEV ? 'cors' : 'same-origin',
		credentials: import.meta.env.DEV ? 'include' : 'same-origin'
	})

	let json = await res.json().catch(() => null);

	if (autonomia)
		executarAcao(res, json);

	return {
		ok: res.ok,
		status: res.status,
		resposta: json
	};
}

async function postAPI(rota, argsQuery = null, argsForm = null, autonomia = true)
{
	return chamarAPI(rota, 'POST', null, argsForm, autonomia);
}


async function getAPI(rota, argsQuery = null, autonomia = true)
{
	return chamarAPI(rota, 'GET', null, null, autonomia);
}

export { postAPI, getAPI };
