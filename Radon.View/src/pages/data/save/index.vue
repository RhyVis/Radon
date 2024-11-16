<script lang="ts" setup>
import { apiPost, apiPutState } from "@/lib/common/apiMethods";
import { useSaveStore } from "@/pages/data/save/scripts/store";
import { storeToRefs } from "pinia";
import { DownloadIcon, UploadIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { useI18n } from "vue-i18n";

const store = useSaveStore();
const { t } = useI18n();
const { loading, id, qText, qNote, rText, rNote } = storeToRefs(store);

const handleStore = async () => {
  if (qText.value.length === 0) {
    await MessagePlugin.warning(t("msg.empty"));
    return;
  }
  loading.value = true;
  try {
    const b = await apiPutState("/api/text-store", store.query);
    if (b) {
      await MessagePlugin.success(t("msg.store"));
    } else {
      await MessagePlugin.error(t("msg.storeFail"));
    }
  } catch (e) {
    console.error(e);
    await MessagePlugin.error(t("msg.storeFail"));
  } finally {
    loading.value = false;
  }
};
const handleSelect = async () => {
  loading.value = true;
  try {
    const data = (
      await apiPost<{
        id: number;
        text: string;
        note: string;
      }>("/api/text-store", store.query)
    ).data;

    rText.value = qText.value = data.text;
    rNote.value = qNote.value = data.note;

    await MessagePlugin.success(t("msg.select"));
  } catch (e) {
    console.error(e);
    await MessagePlugin.error(t("msg.selectFail"));
  } finally {
    loading.value = false;
  }
};
</script>

<template>
  <content-layout :title="t('tt')" :subtitle="t('st')">
    <t-form>
      <t-form-item :label="t('form.store')">
        <t-textarea v-model="qText" :autosize="true" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.note')">
        <t-input v-model="qNote" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.read')">
        <t-textarea :autosize="true" :readonly="true" :value="rText" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.readNote')">
        <t-input :value="rNote" :readonly="true" placeholder="" />
      </t-form-item>
      <t-divider />
      <t-form-item label="ID">
        <t-input-number v-model="id" :auto-width="true" :min="0" size="small" />
      </t-form-item>
      <t-form-item :label="t('form.operation')">
        <t-loading :loading="loading" size="small">
          <t-space :size="12">
            <t-button shape="rectangle" theme="primary" @click="handleStore">
              <UploadIcon />
            </t-button>
            <t-button shape="rectangle" theme="primary" @click="handleSelect">
              <DownloadIcon />
            </t-button>
          </t-space>
        </t-loading>
      </t-form-item>
      <t-form-item :label="t('form.tool')">
        <t-space :size="5">
          <btn-copy :target="rText" />
          <btn-read v-model="qText" />
          <btn-clear v-model="qText" />
        </t-space>
      </t-form-item>
    </t-form>
  </content-layout>
</template>

<i18n locale="en">
tt: "Text Storage"
st: "Key-Value"
form:
  store: "Store Content"
  note: "Store Note"
  read: "Read Content"
  readNote: "Read Note"
  operation: "Data Operation"
  tool: "Tool"
msg:
  empty: "Do not store empty"
  id: "ID cannot be less than 0"
  store: "Store Success"
  storeFail: "Store Fail"
  select: "Select Success"
  selectFail: "Select Fail"
</i18n>

<i18n locale="zh-CN">
tt: "字符存储"
st: "键值对"
form:
  store: "存储内容"
  note: "存储备注"
  read: "读取内容"
  readNote: "读取备注"
  operation: "数据操作"
  tool: "工具"
msg:
  empty: "不可存储空内容"
  id: "ID不可小于0"
  store: "存储成功"
  storeFail: "存储失败"
  select: "选择成功"
  selectFail: "选择失败"
</i18n>
