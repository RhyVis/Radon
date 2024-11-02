<script lang="ts" setup>
import ButtonClear from "@/components/btn/ButtonClear.vue";
import ButtonCopy from "@/components/btn/ButtonCopy.vue";
import ButtonRead from "@/components/btn/ButtonRead.vue";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import { apiPost } from "@/lib/util/apiMethods";
import type { SaveEntry } from "@/pages/data/save/scripts/entryType";
import { useSaveStore } from "@/store/comps/save";
import { DownloadIcon, UploadIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { onMounted, reactive, ref } from "vue";

const saveStore = useSaveStore();

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
    await MessagePlugin.warning("请勿存储空置");
  } else if (query.id < 0) {
    query.id = 0;
    await MessagePlugin.warning("ID不能小于0");
  } else {
    loading.value = true;
    try {
      const dt = (await apiPost("/api/update", query)).data;
      if (dt === 0) {
        result.sign = "存储成功";
        saveStore.update(query.id);
        await MessagePlugin.success("存储成功");
      } else {
        console.error(dt);
        result.sign = "存储失败";
        await MessagePlugin.error("存储失败");
      }
    } catch (e) {
      console.error(e);
      result.sign = "存储失败";
      await MessagePlugin.error("存储失败");
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
      await MessagePlugin.error("ID不能小于0");
    } else {
      const r = (await apiPost("/api/save/query", query)).data as SaveEntry;
      const { id, text, note } = r;
      if (id < 0) {
        result.sign = "读取失败";
        await MessagePlugin.error("读取失败");
      } else {
        result.text = query.text = text;
        result.note = query.note = note;
        result.sign = "读取成功";
        saveStore.update(query.id);
        await MessagePlugin.success("读取成功");
      }
    }
  } catch (e) {
    console.error(e);
    result.sign = "读取失败";
    await MessagePlugin.error("读取失败");
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  query.id = saveStore.id;
});
</script>

<template>
  <ContentLayout title="字符存储" subtitle="键值对">
    <t-form>
      <t-form-item label="存储内容">
        <t-textarea v-model="query.text" :autosize="true" placeholder="存储内容" />
      </t-form-item>
      <t-form-item label="存储备注">
        <t-input v-model="query.note" placeholder="存储备注" />
      </t-form-item>
      <t-form-item label="读取内容">
        <t-textarea :autosize="true" :readonly="true" :value="result.text" placeholder="读取内容" />
      </t-form-item>
      <t-form-item label="读取备注">
        <t-input :value="result.note" :readonly="true" placeholder="读取备注" />
      </t-form-item>
      <t-divider />
      <t-form-item label="ID">
        <t-input-number v-model="query.id" :auto-width="true" :min="0" size="small" />
      </t-form-item>
      <t-form-item label="数据操作">
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
      <t-form-item label="工具">
        <t-space :size="5">
          <ButtonCopy :target="result.text" />
          <ButtonRead v-model:target="query.text" />
          <ButtonClear v-model:target="query.text" />
        </t-space>
      </t-form-item>
    </t-form>
  </ContentLayout>
</template>
