import { enqueueSnackbar } from "notistack";

function mostrarErro(res, json) {
  if (!res.ok && json?.detalhes)
    enqueueSnackbar(json.detalhes, { variant: "error" });
}

async function chamarAPI(rota, verbo, argsQuery = null, argsForm = null) {
  rota =
    import.meta.env.VITE_URL_API +
    `${rota}${argsQuery ? `?${new URLSearchParams(argsQuery)}` : "/"}`;
  verbo = verbo.trim().toUpperCase();

  let corpo = null;
  let cabecalhos = {};

  if (verbo != "GET" && argsForm) {
    cabecalhos["Content-Type"] = "application/x-www-form-urlencoded";
    corpo = new URLSearchParams(argsForm);
  }

  let res = await fetch(rota, {
    method: verbo,
    headers: cabecalhos,
    body: corpo,
    mode: import.meta.env.DEV ? "cors" : "same-origin",
    credentials: import.meta.env.DEV ? "include" : "same-origin",
  });

  let json = await res.json().catch(() => null);

  mostrarErro(res, json);

  return {
    ok: res.ok,
    status: res.status,
    resposta: json,
  };
}

export { chamarAPI };
