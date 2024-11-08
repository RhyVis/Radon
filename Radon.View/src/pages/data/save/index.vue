<script lang="ts" setup>
import ButtonClear from "@/components/btn/ButtonClear.vue";
import ButtonCopy from "@/components/btn/ButtonCopy.vue";
import ButtonRead from "@/components/btn/ButtonRead.vue";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { apiPost, apiPut } from "@/lib/common/apiMethods";
import type { SaveEntry } from "@/pages/data/save/scripts/entryType";
import { useSaveStore } from "@/pages/data/save/scripts/store";
import { DownloadIcon, UploadIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const saveStore = useSaveStore();
const { t } = useI18n();

const query = reactive({
  id: 0,
  text: "",
  note: "",
});
const result = reactive({
  text: "",
  note: "",
  sign: "---",
});
const loading = ref(false);

const handleStore = async () => {
  result.sign = "---";
  if (query.text.length === 0) {
    await MessagePlugin.warning(t("msg.empty"));
  } else if (query.id < 0) {
    query.id = 0;
    await MessagePlugin.warning(t("msg.id"));
  } else {
    loading.value = true;
    try {
      const dt = (await apiPost("/api/text-store", query)).data;
      if (dt === 0) {
        result.sign = t("msg.store");
        saveStore.update(query.id);
        await MessagePlugin.success(t("msg.store"));
      } else {
        console.error(dt);
        result.sign = t("msg.storeFail");
        await MessagePlugin.error(t("msg.storeFail"));
      }
    } catch (e) {
      console.error(e);
      result.sign = t("msg.storeFail");
      await MessagePlugin.error(t("msg.storeFail"));
    } finally {
      loading.value = false;
    }
  }
};
const handleSelect = async () => {
  loading.value = true;
  try {
    if (query.id < 0) {
      query.id = 0;
      await MessagePlugin.error(t("msg.id"));
    } else {
      const r = (await apiPut("/api/text-store", query)).data as SaveEntry;
      const { id, text, note } = r;
      if (id < 0) {
        result.sign = t("msg.selectFail");
        await MessagePlugin.error(t("msg.selectFail"));
      } else {
        result.text = query.text = text;
        result.note = query.note = note;
        result.sign = t("msg.select");
        saveStore.update(query.id);
        await MessagePlugin.success(t("msg.select"));
      }
    }
  } catch (e) {
    console.error(e);
    result.sign = t("msg.selectFail");
    await MessagePlugin.error(t("msg.selectFail"));
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  query.id = saveStore.id;
});
</script>

<template>
  <ContentLayout :title="t('tt')" :subtitle="t('st')">
    <t-form>
      <t-form-item :label="t('form.store')">
        <t-textarea v-model="query.text" :autosize="true" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.note')">
        <t-input v-model="query.note" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.read')">
        <t-textarea :autosize="true" :readonly="true" :value="result.text" placeholder="" />
      </t-form-item>
      <t-form-item :label="t('form.readNote')">
        <t-input :value="result.note" :readonly="true" placeholder="" />
      </t-form-item>
      <t-divider />
      <t-form-item label="ID">
        <t-input-number v-model="query.id" :auto-width="true" :min="0" size="small" />
      </t-form-item>
      <t-form-item :label="t('form.operation')">
        <t-loading :loading="loading" size="small">
          <t-space>
            <t-button shape="circle" theme="primary" @click="handleStore">
              <UploadIcon />
            </t-button>
            <t-button shape="circle" theme="primary" @click="handleSelect">
              <DownloadIcon />
            </t-button>
          </t-space>
        </t-loading>
      </t-form-item>
      <t-form-item :label="t('form.tool')">
        <t-space :size="5">
          <ButtonCopy :target="result.text" />
          <ButtonRead v-model:target="query.text" />
          <ButtonClear v-model:target="query.text" />
        </t-space>
      </t-form-item>
    </t-form>
  </ContentLayout>
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
