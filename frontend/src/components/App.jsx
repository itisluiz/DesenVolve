import { HashRouter, Route, Routes } from 'react-router-dom'
import Contexto from './Contexto';
import Layout from './Layout';

import PaginaHome from '../pages/PaginaHome';
import PaginaAutenticar from '../pages/PaginaAutenticar';

function App() {
	return (
		<Contexto>
			<Layout>
				<HashRouter>
					<Routes>
						<Route path="/" element={<PaginaHome />} />
						<Route path="/autenticar" element={<PaginaAutenticar />} />
					</Routes>
				</HashRouter>
			</Layout>
		</Contexto>
	)
}

export default App
