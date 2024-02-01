import { HashRouter, Route, Routes } from 'react-router-dom'
import Contexo from './Contexto';
import Layout from './Layout';

import PaginaHome from '../pages/PaginaHome';

function App() {
	return (
		<Contexo>
			<Layout>
				<HashRouter>
					<Routes>
						<Route path="/" element={<PaginaHome />} />
					</Routes>
				</HashRouter>
			</Layout>
		</Contexo>
	)
}

export default App
