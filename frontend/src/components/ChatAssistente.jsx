import { Icon, IconButton, Paper, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import { apiCriarConversa, apiMensagemConversa } from "../api/assistente";
import { Stack } from "@mui/system";

import CardMensagem from "./Cards/CardMensagem";

const ChatAssistente = (props) => {
  const [assistente, setAssistente] = useState(null);
  const [mensagemEnviar, setMensagemEnviar] = useState("");
  const [esperandoAssistente, setEsperandoAsssitente] = useState(false);
  const [listaMensagens, setListaMensagens] = useState([]);

  function submitCriarAssistente() {
    setEsperandoAsssitente(true);
    apiCriarConversa(props.codigoProjeto).then((res) => {
      if (!res) return;

      setAssistente(res.idAssistente);
      apiMensagemConversa(res.idAssistente, null).then((res) => {
        if (!res) return;

        setListaMensagens([
          ...listaMensagens,
          { usuario: false, mensagem: res.resultado },
        ]);
        setEsperandoAsssitente(false);
      });
    });
  }

  function submitEnviarMensagem() {
    setEsperandoAsssitente(true);
    setListaMensagens([
      ...listaMensagens,
      { usuario: true, mensagem: mensagemEnviar },
    ]);

    let listaMensagensAtt = [
      ...listaMensagens,
      { usuario: true, mensagem: mensagemEnviar },
    ];
    apiMensagemConversa(assistente, mensagemEnviar).then((res) => {
      if (!res) return;

      setListaMensagens([
        ...listaMensagensAtt,
        { usuario: false, mensagem: res.resultado },
      ]);
      setEsperandoAsssitente(false);
    });

    setMensagemEnviar("");
  }

  useEffect(() => {
    submitCriarAssistente();
  }, []);

  return (
    <Stack gap="1em">
      <Paper
        sx={{ width: "30em", height: "30em", overflow: "scroll", pb: "1em" }}
      >
        {listaMensagens.map((msg) => (
          <CardMensagem
            key={msg.mensagem}
            usuario={msg.usuario}
            mensagem={msg.mensagem}
          />
        ))}
        <CardMensagem hidden={!esperandoAssistente} />
      </Paper>
      <div style={{ display: "flex", width: "100%" }}>
        <TextField
          value={mensagemEnviar}
          onChange={(e) => setMensagemEnviar(e.target.value)}
          fullWidth
          placeholder="Digite sua mensagem"
          variant="standard"
        >
          dsadsa
        </TextField>
        <IconButton
          type="submit"
          onClick={submitEnviarMensagem}
          disabled={esperandoAssistente || mensagemEnviar.length == 0}
          color="primary"
        >
          <Icon>send</Icon>
        </IconButton>
      </div>
    </Stack>
  );
};

export default ChatAssistente;
