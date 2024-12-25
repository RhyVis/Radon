import axiosInstance from "@/lib/common/apiHttp";
import { getAuthPassport } from "@/lib/common/authMethods.ts";

const IMAGE_UPLOAD_URL = import.meta.env.VITE_IMAGE_UPLOAD_URL ?? "/api/upload/image";

export type ImageUploadResponse = {
  result: string;
  code: number;
  url: string;
  srcName: string;
  thumb: string;
  del: string;
};

/**
 * Upload an image to the server.
 * @param file The image file to upload.
 * @param token The token used to upload the image, if not provided, the token stored in localStorage will be used.
 */
export async function apiUploadImage(file: File, token?: string): Promise<ImageUploadResponse> {
  const formData = new FormData();

  formData.append("image", file);
  if (token) {
    formData.append("token", token);
  } else {
    const image_token = getAuthPassport()?.extra.imageToken;
    if (image_token) formData.append("token", image_token);
  }

  console.log(`Trying to upload image: ${file.name} with token: ${token}`);
  const response = await axiosInstance.post(IMAGE_UPLOAD_URL, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
  const data = response.data as ImageUploadResponse;
  if (data.code !== 200) {
    console.warn(`Image upload error: ${data.code}`);
    console.log(data);
  }

  return data;
}

/**
 * Upload a list of images to the server.
 * @param files The list of image files to upload.
 * @param token The token used to upload the images, if not provided, the token stored in localStorage will be used.
 */
export async function apiUploadImageList(files: File[], token?: string): Promise<ImageUploadResponse[]> {
  return await Promise.all(files.map(file => apiUploadImage(file, token)));
}
