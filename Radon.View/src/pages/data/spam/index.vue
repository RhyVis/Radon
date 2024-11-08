<script setup lang="tsx">
import { apiPost, apiPut } from "@/lib/common/apiMethods";
import { MessagePlugin, type TableProps } from "tdesign-vue-next";
import { useSpamStore } from "@/pages/data/spam/scripts/store";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import ButtonCopy from "@/components/btn/ButtonCopy.vue";
import type { TextEntry } from "@/lib/type/typeEntry";
import useClipboard from "vue-clipboard3";
import { CloseIcon, HomeIcon, PlayCircleStrokeAddIcon, ReplayIcon, ToolsIcon } from "tdesign-icons-vue-next";
import { useGlobalStore } from "@/store/global";
import SelSimple from "@/components/select/SelSimple.vue";
import { spamTypes, codeTypes } from "@/pages/data/spam/scripts/type";

const global = useGlobalStore();
const store = useSpamStore();
const { qType, qDict, qSize, aType, aText, activeTab } = storeToRefs(store);
const { toClipboard } = useClipboard();

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
const showAppendingDialog = ref(false);
const appendLoading = ref(false);

const handleTabChange = (key: string | number) => {
  switch (key) {
    case "spam":
      qType.value = "sn";
      break;
    case "mmr":
      qType.value = "gs";
      break;
    case "meme":
      qType.value = "ac";
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
    result.value = (await apiPost("/api/spam", store.query)).data as TextEntry[];
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("获取信息失败");
  } finally {
    resultLoading.value = false;
  }
};
const handleAppend = async (repeat: boolean = false) => {
  if (aText.value.length === 0) {
    await MessagePlugin.error("内容不能为空");
    return;
  }
  appendLoading.value = true;
  try {
    const r = (await apiPut("/api/spam", store.queryAppend)).data as number;
    if (r === 0) {
      store.clearAppendQuery();
      await MessagePlugin.success("追加成功");
    } else {
      await MessagePlugin.error("追加失败");
    }
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("追加失败");
  } finally {
    appendLoading.value = false;
    if (!repeat) {
      setTimeout(() => (showAppendingDialog.value = false), 666);
    }
  }
};
</script>

<template>
  <ContentLayout title="弹药库" subtitle="对线宝典">
    <t-form>
      <t-tabs v-model="activeTab" @change="handleTabChange">
        <!--祖安-->
        <t-tab-panel class="mt-2" label="祖安特区" value="spam">
          <t-form-item label="使用说明">
            <span class="r-sp-panel-desc">高强度版本容易被夹，建议转义</span>
          </t-form-item>
          <t-form-item label="选择强度">
            <t-radio-group v-model="qType">
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
            <t-radio-group v-model="qType">
              <t-tooltip content="原神" placement="top">
                <t-radio-button value="gs">原批</t-radio-button>
              </t-tooltip>
              <t-tooltip content="明日方舟" placement="top">
                <t-radio-button value="ak">粥畜</t-radio-button>
              </t-tooltip>
              <t-tooltip content="麻辣香锅" placement="top">
                <t-radio-button value="ml">麻辣</t-radio-button>
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
            <t-radio-group v-model="qType" type="button">
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
          <SelSimple v-model:selected="qDict" :options="codeTypes" />
        </t-form-item>
        <t-form-item label="妙语连珠">
          <t-input-number v-model="qSize" :min="1" :max="10" :auto-width="true" :allow-input-over-limit="false" />
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
    <template #actions>
      <t-button
        v-if="global.authPassed"
        variant="text"
        shape="circle"
        theme="primary"
        size="small"
        @click="showAppendingDialog = true"
      >
        <ToolsIcon />
      </t-button>
      <RouterLink to="/">
        <t-button variant="text" theme="primary" shape="circle" size="small">
          <HomeIcon />
        </t-button>
      </RouterLink>
    </template>
    <t-dialog v-model:visible="showAppendingDialog" header="追加内容" width="75%" @close="store.clearAppendQuery()">
      <t-form label-align="top">
        <t-form-item label="类型">
          <SelSimple v-model:selected="aType" :options="spamTypes" />
        </t-form-item>
        <t-form-item label="内容">
          <t-textarea v-model="aText" />
        </t-form-item>
      </t-form>
      <template #footer>
        <t-space>
          <ButtonRead v-model:target="aText" />
          <t-button theme="default" shape="round" @click="showAppendingDialog = false">
            <CloseIcon />
          </t-button>
          <t-button theme="primary" shape="round" @click="handleAppend()" :loading="appendLoading">
            <PlayCircleStrokeAddIcon v-if="!appendLoading" />
          </t-button>
          <t-button theme="primary" shape="round" @click="handleAppend(true)" :loading="appendLoading">
            <ReplayIcon v-if="!appendLoading" />
          </t-button>
        </t-space>
      </template>
    </t-dialog>
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
