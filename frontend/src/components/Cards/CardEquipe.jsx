import { Button, Card, CardContent, Icon, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

const CardEquipe = (props) => {
  const navigate = useNavigate();
  return (
    <Card>
      <CardContent sx={{ display: "flex" }}>
        <div>
          <Typography variant="h5" component="h2">
            <Icon color="primary" sx={{ mr: "0.35em", mb: "-0.2em" }}>
              group
            </Icon>
            {props.equipe.nome}
          </Typography>
          <Typography color="textSecondary">
            Seu cargo nesta equipe: <b>{props.equipe.cargo.nome}</b>
          </Typography>
        </div>
        <Button
          onClick={() => navigate(`/equipe/${props.equipe.codigo}`)}
          sx={{ marginLeft: "auto" }}
        >
          Acessar página
          <Icon sx={{ marginLeft: "0.35em" }}>arrow_forward</Icon>
        </Button>
      </CardContent>
    </Card>
  );
};

export default CardEquipe;
