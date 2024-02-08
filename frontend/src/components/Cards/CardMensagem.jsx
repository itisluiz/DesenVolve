import { Icon, LinearProgress, Typography } from "@mui/material";
import { Container } from "@mui/system";

const CardMensagem = (props) => {
  let icone = props.usuario ? "person" : "smart_toy";
  let cor = props.usuario ? "#EEEEEE" : "#8ED1C0";

  if (props.hidden) return;

  return (
    <Container sx={{ textWrap: "wrap", mt: "1em" }}>
      <Typography sx={{ background: cor, p: "1em", borderRadius: "1em" }}>
        <Icon sx={{ m: "0em 0.4em -0.2em 0em" }}>{icone}</Icon>
        {props.mensagem ?? <LinearProgress color="secondary" />}
      </Typography>
    </Container>
  );
};

export default CardMensagem;
