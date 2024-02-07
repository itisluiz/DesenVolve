import { enqueueSnackbar } from 'notistack'

function executarAcao(res, json) {

    if (!res.ok)
    {
        if (json && json.detalhes)
            enqueueSnackbar(json.detalhes, { variant: 'error', autoHideDuration: 3000 });
        else
            enqueueSnackbar(`Erro de status ${res.status}`, { variant: 'error', autoHideDuration: 3000 });
    }
}

export { executarAcao }
