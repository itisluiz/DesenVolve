import { chamarAPI } from "./api";

async function apiLogin(email, senha) {
  let res = await chamarAPI("login", "POST", null, {
    email: email,
    senha: senha,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiCadastro(nome, sobrenome, email, senha) {
  let res = await chamarAPI("usuario", "POST", null, {
    nome: nome,
    sobrenome: sobrenome,
    email: email,
    senha: senha,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiLogout() {
  await chamarAPI("logout", "POST");
}

async function apiLogado() {
  let res = await chamarAPI("usuario", "GET");

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiEquipes() {
  let res = await chamarAPI("usuario/equipes", "GET");

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

export { apiLogin, apiCadastro, apiLogout, apiLogado, apiEquipes };
