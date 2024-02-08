import { chamarAPI } from "./api";

async function apiCriarTarefa(
  codigoProjeto,
  nome,
  descricao,
  complexidade,
  codigoResponsavel,
  prazo
) {
  let res = await chamarAPI("tarefa", "POST", null, {
    codigoProjeto: codigoProjeto,
    nome: nome,
    descricao: descricao,
    complexidade: complexidade,
    codigoResponsavel: codigoResponsavel,
    prazo: prazo,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiIniciarTarefa(codigoTarefa) {
  let res = await chamarAPI("tarefa/iniciar", "POST", null, {
    codigoTarefa: codigoTarefa,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

async function apiFinalizarTarefa(codigoTarefa) {
  let res = await chamarAPI("tarefa/finalizar", "POST", null, {
    codigoTarefa: codigoTarefa,
  });

  if (!res.ok || !res.resposta) return null;

  return res.resposta;
}

export { apiCriarTarefa, apiFinalizarTarefa, apiIniciarTarefa };
