import http from 'k6/http';
import { check, sleep } from 'k6';
import { textSummary } from "https://jslib.k6.io/k6-summary/0.1.0/index.js";

export let options = {
    insecureSkipTLSVerify: true,
    thresholds: {
        http_req_failed: ['rate<0.05']
    }    
};

export default function () {
  const baseUrl = 'http://localhost:5148';
  const getEstadosResponse = http.get(`${baseUrl}/estados`);  
  check(getEstadosResponse, {
    '/estados: status é 200 OK': (r) => r.status === 200,
  });

  const siglasEstados = ['SP', 'RS', 'MS', 'BA', 'PA'];
  for (const siglaEstado of siglasEstados) {
    const getEstadoResponse = http.get(`${baseUrl}/estados/${siglaEstado}`);  
    check(getEstadoResponse, {
      [`/estados/${siglaEstado}: status é 200 OK`]: (r) => r.status === 200,
    });
  }

  const siglasRegioes = ['SE', 'NE', 'CO', 'N', 'S'];
  for (const siglaRegiao of siglasRegioes) {
    const getRegioesResponse = http.get(`${baseUrl}/regioes/${siglaRegiao}`);  
    check(getRegioesResponse, {
      [`/regioes/${siglaRegiao}: status é 200 OK`]: (r) => r.status === 200,
    });
  }
  
  // Espera 1 segundo ao final de cada iteração
  sleep(1);
}

export function handleSummary(data) {
  return {
    stdout: textSummary(data, { indent: " ", enableColors: true }),
  };
}