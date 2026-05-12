import axios, { AxiosResponse } from "axios";
import { ZodSchema } from "zod";


const baseURL=process.env.API_BASE
export const Axios=axios.create({
    url:baseURL,
    headers: { 'Content-Type': 'application/json' }
})

declare module 'axios' {
  export interface AxiosRequestConfig {
    zodSchema?: ZodSchema<any>;
  }
}
Axios.interceptors.request.use((config)=>{
    const token=localStorage.getItem("token")
    if(!token)throw Error
    config.headers.Authorization=`Bearer ${token}`
    return config
})

Axios.interceptors.response.use(
  (response: AxiosResponse) => {
    const schema = response.config.zodSchema;
    
    if (schema) {
      try {
        // Parse the response data with the provided schema
        const parsed = schema.parse(response.data);
        // Attach parsed data to a custom property
        (response as any).parsedData = parsed;
      } catch (error) {
        console.error("Zod validation failed:", error);
        return Promise.reject(error);
      }
    }
    
    return response;
  },
  (error) => Promise.reject(error)
);