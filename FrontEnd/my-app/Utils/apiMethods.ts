// apiMethods.ts
import { Axios } from "./axios";
import { ZodSchema } from "zod";

// Generic GET with optional Zod validation
export const GET = async <T, P = Record<string, unknown>>(
  url: string,
  params?: P,
  schema?: ZodSchema<T>
): Promise<T> => {
  const res = await Axios.get<T>(url, { params, zodSchema: schema });
  // Return parsed data if available, otherwise return raw data
  return (res as any).parsedData || res.data;
};

// Generic POST
export const POST = async <T, B = any>(
  url: string,
  body: B,
  schema?: ZodSchema<T>
): Promise<T> => {
  const res = await Axios.post<T>(url, body, { zodSchema: schema });
  return (res as any).parsedData || res.data;
};

// Generic DELETE
export const DELETE = async <T>(
  url: string,
  schema?: ZodSchema<T>
): Promise<T> => {
  const res = await Axios.delete<T>(url, { zodSchema: schema });
  return (res as any).parsedData || res.data;
};

// Generic PUT
export const PUT = async <T, B = any>(
  url: string,
  body: B,
  schema?: ZodSchema<T>
): Promise<T> => {
  const res = await Axios.put<T>(url, body, { zodSchema: schema });
  return (res as any).parsedData || res.data;
};

// Generic PATCH
export const PATCH = async <T, B = any>(
  url: string,
  body: B,
  schema?: ZodSchema<T>
): Promise<T> => {
  const res = await Axios.patch<T>(url, body, { zodSchema: schema });
  return (res as any).parsedData || res.data;
};