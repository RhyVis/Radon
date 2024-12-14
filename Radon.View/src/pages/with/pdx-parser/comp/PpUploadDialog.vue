<script lang="ts" setup>
import { apiPostWithFile } from "@/lib/common/apiMethods.ts";
import type { PdxLangEventItem, PdxLangItem } from "@/pages/with/pdx-parser/scripts/define.ts";
import {
  buildPdxLangTree,
  getPdxLangParsedEventItemById,
  getPdxLangParsedItemById,
} from "@/pages/with/pdx-parser/scripts/function.ts";
import { usePdxStore } from "@/pages/with/pdx-parser/scripts/store.ts";
import { useGlobalStore } from "@/store/global.ts";
import { get, set, useToggle } from "@vueuse/core";
import { storeToRefs } from "pinia";
import { DownloadIcon, Upload1Icon } from "tdesign-icons-vue-next";
import { MessagePlugin, type RequestMethodResponse, type UploadFile } from "tdesign-vue-next";
import type { TypeTreeOptionData } from "tdesign-vue-next/es/tree/adapt";
import { computed } from "vue";
import { useI18n } from "vue-i18n";

const { t } = useI18n();

const { type } = defineProps<{
  type: "event" | "tree";
}>();

const { authPassed } = storeToRefs(useGlobalStore());
const { requestTextStorageId, eventSelectId } = storeToRefs(usePdxStore());

const menuUploadVisible = defineModel<boolean>("menuUploadVisible", { required: true, default: false });

const parseResultEvent = defineModel<PdxLangEventItem[] | undefined>("parseResultEvent");
const parseResultTree = defineModel<TypeTreeOptionData[] | undefined>("parseResultTree");

const requestTextStorageBtnLabel = computed(() => (type === "event" ? t("upload.dbEvent") : t("upload.dbTree")));

const [fileUploading, setFileUploading] = useToggle(false);
const [idFetchLoading, setIdFetchLoading] = useToggle(false);

const parse = (list: PdxLangItem[]) => set(parseResultTree, buildPdxLangTree(list));
const requestFileMethod = async (file: UploadFile): Promise<RequestMethodResponse> => {
  try {
    setFileUploading(true);
    switch (type) {
      case "tree": {
        const { data } = await apiPostWithFile<PdxLangItem[]>(
          "/api/pdx/parse/lang",
          new Blob([file.raw!], { type: file.type }),
        );

        parse(data);
        set(parseResultEvent, []);

        break;
      }
      case "event": {
        const { data } = await apiPostWithFile<PdxLangEventItem[]>(
          "/api/pdx/parse/event",
          new Blob([file.raw!], { type: file.type }),
        );

        set(parseResultEvent, data);
        set(eventSelectId, 0);
        set(parseResultTree, []);

        break;
      }
      default: {
        void MessagePlugin.warning("Not Implemented");
        break;
      }
    }

    set(menuUploadVisible, false);

    return { status: "success", response: { success: true } };
  } catch (e) {
    console.error(e);
    return { status: "fail", error: "x", response: {} };
  } finally {
    setFileUploading(false);
  }
};
const requestIdMethod = async () => {
  if (get(requestTextStorageId) <= 0) {
    void MessagePlugin.warning(t("msg.idInvalid"));
    return;
  }
  try {
    setIdFetchLoading(true);
    switch (type) {
      case "tree": {
        const d = await getPdxLangParsedItemById(get(requestTextStorageId));
        parse(d);

        set(eventSelectId, 0);
        set(parseResultEvent, []);
        set(menuUploadVisible, false);

        break;
      }
      case "event": {
        const d = await getPdxLangParsedEventItemById(get(requestTextStorageId));

        set(eventSelectId, 0);
        set(parseResultEvent, d);
        set(parseResultTree, []);
        set(menuUploadVisible, false);

        break;
      }
    }
  } catch (e) {
    console.error(e);
  } finally {
    setIdFetchLoading(false);
  }
};
</script>

<template>
  <t-dialog v-model:visible="menuUploadVisible" :footer="false" :header="t('upload.title')" width="85%">
    <div class="w-full">
      <t-space v-if="!idFetchLoading" align="baseline" direction="vertical">
        <t-title :content="t('upload.s1')" level="h6" />
        <t-upload
          :placeholder="t('upload.placeholder')"
          :request-method="requestFileMethod"
          :tips="t('upload.tips')"
          theme="file-input"
        >
          <t-button shape="circle" theme="default" variant="outline">
            <Upload1Icon />
          </t-button>
        </t-upload>
      </t-space>
      <template v-if="authPassed && !fileUploading">
        <t-divider v-if="!idFetchLoading" />
        <t-title :content="t('upload.s2')" level="h6" />
        <t-form label-align="top">
          <t-form-item label="ID">
            <t-input v-model="requestTextStorageId" :auto-width="true" />
          </t-form-item>
          <t-form-item :label="requestTextStorageBtnLabel">
            <t-button :loading="idFetchLoading" theme="primary" @click="requestIdMethod">
              <DownloadIcon v-if="!idFetchLoading" />
            </t-button>
          </t-form-item>
        </t-form>
      </template>
    </div>
  </t-dialog>
</template>

<i18n locale="en">
upload:
  title: Upload to Parse
  s1: File Upload
  s2: Database
  placeholder: Paradox Yaml Lang
  tips: Support YAML, do not upload the file repeatedly, your browser can't handle it
  dbTree: Parse Tree from Database
  dbEvent: Parse Event from Database
msg:
  idInvalid: ID Invalid
</i18n>

<i18n locale="zh-CN">
upload:
  title: 上传解析源
  s1: 文件上传
  s2: 数据库
  placeholder: Paradox Yaml Lang
  tips: 支持 YAML，不要重复上传文件，你的浏览器受不了
  dbTree: 从数据库解析树
  dbEvent: 从数据库解析事件
msg:
  idInvalid: ID 无效
</i18n>
