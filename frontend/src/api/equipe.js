import { chamarAPI } from "./api";

async function apiObterEquipe(codigoEquipe) {
  let res = await chamarAPI("equipe", "GET", { codigoEquipe: codigoEquipe });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiCriarEquipe(nome) {
  let res = await chamarAPI("equipe", "POST", null, { nome: nome });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiProjetos(codigoEquipe) {
  let res = await chamarAPI("equipe/projetos", "GET", {
    codigoEquipe: codigoEquipe,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiAlterarCargo(codigoEquipe, codigoUsuario, cargo) {
  let res = await chamarAPI("equipe/membro", "PATCH", null, {
    codigoEquipe: codigoEquipe,
    codigoUsuario: codigoUsuario,
    cargo: cargo,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiRemoverMembro(codigoEquipe, codigoUsuario) {
  let res = await chamarAPI("equipe/membro", "DELETE", null, {
    codigoEquipe: codigoEquipe,
    codigoUsuario: codigoUsuario,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiAdicionarMembro(codigoEquipe, emailUsuario, cargo) {
  let res = await chamarAPI("equipe/membro", "POST", null, {
    codigoEquipe: codigoEquipe,
    emailUsuario: emailUsuario,
    cargo: cargo,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiRemoverEquipe(codigoEquipe) {
  let res = await chamarAPI("equipe", "DELETE", null, {
    codigoEquipe: codigoEquipe,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

export {
  apiCriarEquipe,
  apiProjetos,
  apiObterEquipe,
  apiAlterarCargo,
  apiRemoverMembro,
  apiAdicionarMembro,
  apiRemoverEquipe,
};
