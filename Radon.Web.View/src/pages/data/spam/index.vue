<script setup lang="tsx">
import { onMounted, reactive, ref } from "vue";
import { apiPost } from "@/lib/util/apiMethods";
import { MessagePlugin, type TableProps } from "tdesign-vue-next";
import { useSpamStore } from "@/pages/data/spam/scripts/store";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import SelectSimple from "@/components/select/SelectSimple.vue";
import ButtonCopy from "@/components/btn/ButtonCopy.vue";
import type { TextEntry } from "@/lib/type/typeEntry";
import useClipboard from "vue-clipboard3";

const store = useSpamStore();
const { toClipboard } = useClipboard();

const query = reactive({
  type: "sn",
  dict: "none",
  size: 1,
});
const conf = reactive({
  activeTab: "spam",
});
const resultLoading = ref(false);

const result = ref<TextEntry[]>([
  { id: 666, text: "快乐生活每一天，请不要用这个工具的结果来攻击他人哦😊" },
  { id: 999, text: "仅供学习交流使用，由您不当使用造成的后果，将由您承担" },
]);

const columns = ref<TableProps["columns"]>([
  {
    colKey: "id",
    title: "ID",
    width: 60,
    cell: (_, { row }) => {
      return (
        <div class="r-sp-column-tag" onClick={() => handleCopy(row.text)}>
          <t-tag shape="round" variant="outline">
            {row.id}
          </t-tag>
        </div>
      );
    },
  },
  {
    colKey: "text",
    title: "内容",
    cell: (_, { row }) => {
      return (
        <t-space direction="vertical" size={2}>
          {(row.text as string).split(/[\n\r]|\r\n|\\r\\n/).map((t, i) => (
            <t-text key={i}>{t}</t-text>
          ))}
        </t-space>
      );
    },
  },
]);
const codeTypes = [
  { value: "none", label: "直白对决😅" },
  { value: "nmsl", label: "抽象加密🤗" },
  { value: "trad", label: "繁体传统🤔" },
  { value: "sprk", label: "火星密文😘" },
  { value: "diff", label: "形近转换🧐" },
];

const changeTab = (key: string | number) => {
  switch (key) {
    case "spam":
      query.type = "sn";
      break;
    case "mmr":
      query.type = "gs";
      break;
    case "meme":
      query.type = "ac";
      break;
    default:
  }
};

const handleCopy = (s: string) => {
  try {
    toClipboard(s.replace(/[\n\r]|\r\n|\\r\\n/, ""));
    MessagePlugin.success("复制成功");
  } catch (e) {
    console.error(e);
    MessagePlugin.error("复制失败");
  }
};
const handleSpam = async () => {
  resultLoading.value = true;
  try {
    result.value = (await apiPost("/api/spam", query)).data as TextEntry[];
    store.update(query, conf);
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("获取信息失败");
  } finally {
    resultLoading.value = false;
  }
};

onMounted(() => {
  query.type = store.type;
  query.dict = store.dict;
  query.size = store.size;
  conf.activeTab = store.activeTab;
});
</script>

<template>
  <ContentLayout title="弹药库" subtitle="对线宝典">
    <t-form>
      <t-tabs v-model="conf.activeTab" @change="changeTab">
        <!--祖安-->
        <t-tab-panel class="mt-2" label="祖安特区" value="spam">
          <t-form-item label="使用说明">
            <span class="r-sp-panel-desc">高强度版本容易被夹，建议转义</span>
          </t-form-item>
          <t-form-item label="选择强度">
            <t-radio-group v-model="query.type">
              <t-radio-button value="sn">小喷怡情😋</t-radio-button>
              <t-radio-button value="sx">火力全开😠</t-radio-button>
            </t-radio-group>
          </t-form-item>
        </t-tab-panel>
        <!--MMR-->
        <t-tab-panel class="mt-2" label="二游笑话" value="mmr">
          <t-form-item label="使用说明">
            <span class="r-sp-panel-desc">
              介于各路孝子挂对面的时候都是截图挂人，
              所以这数据库里面很多东西也都是OCR扫出来的，有错字就当二游痴子没文化吧
            </span>
          </t-form-item>
          <t-form-item label="选择游戏">
            <t-radio-group v-model="query.type">
              <t-tooltip content="原神" placement="top">
                <t-radio-button value="gs">原神怎么你了</t-radio-button>
              </t-tooltip>
              <t-tooltip content="明日方舟" placement="top">
                <t-radio-button value="ak">二游半壁江山</t-radio-button>
              </t-tooltip>
            </t-radio-group>
          </t-form-item>
        </t-tab-panel>
        <!--Meme-->
        <t-tab-panel class="mt-2" label="复制粘贴" value="meme">
          <t-form-item label="使用说明">
            <span class="r-sp-panel-desc">我喜欢复制粘贴</span>
          </t-form-item>
          <t-form-item label="选择主题">
            <t-radio-group v-model="query.type" type="button">
              <t-tooltip content="二次元欠图了" placement="top">
                <t-radio-button value="ac">反二圣经</t-radio-button>
              </t-tooltip>
              <t-tooltip content="不知道该怎么分类了" placement="top">
                <t-radio-button value="dn">纯正低能</t-radio-button>
              </t-tooltip>
            </t-radio-group>
          </t-form-item>
        </t-tab-panel>
      </t-tabs>
      <div class="mt-4">
        <t-form-item label="转义方式">
          <SelectSimple v-model:selected="query.dict" :options="codeTypes" />
        </t-form-item>
        <t-form-item label="妙语连珠">
          <t-input-number v-model="query.size" :min="1" :max="10" :auto-width="true" :allow-input-over-limit="false" />
        </t-form-item>
        <t-form-item label="开火！">
          <t-space>
            <t-button shape="round" theme="warning" variant="outline" @click="handleSpam">😤</t-button>
            <ButtonCopy :target="result.map(t => t.text).join('\n')" />
          </t-space>
        </t-form-item>
      </div>
    </t-form>
    <!--结果-->
    <t-divider />
    <div class="mb-2 mt-2" v-if="result.length > 0">
      <t-table
        size="small"
        row-key="id"
        :columns="columns"
        :data="result"
        :stripe="true"
        :hover="true"
        :bordered="true"
        :loading="resultLoading"
      />
    </div>
  </ContentLayout>
</template>

<style scoped lang="less">
@import "@/assets/style/mixin.less";

:global(.r-sp-column-tag) {
  text-align: center;
  .r-no-select();
}

.r-sp-panel-desc {
  text-align: left;
}
</style>
