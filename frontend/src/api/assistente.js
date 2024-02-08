import { chamarAPI } from "./api";

async function apiCriarConversa(codigoProjeto) {
  let res = await chamarAPI("assistente/nova", "POST", null, {
    codigoProjeto: codigoProjeto,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiMensagemConversa(idAssistente, mensagem) {
  let res = await chamarAPI("assistente", "POST", null, {
    idAssistente: idAssistente,
    mensagem: mensagem,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

export { apiCriarConversa, apiMensagemConversa };
